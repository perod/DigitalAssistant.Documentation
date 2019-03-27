using PackageAnalyzer.Core.Model;
using PackageAnalyzer.Core.Services;
using PackageAnalyzer.WinForm.Events;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System;

namespace PackageAnalyzer.WinForm.Services
{
    public class ViewerService : IViewerService
    {
        private readonly IApplicationStorageService _applicationStorageService;
        private readonly IHtmlRenderer _htmlRenderer;
        private readonly ISolutionListTreeViewService _solutionListTreeViewService;

        private SynchronizationContext _synchronizationContext;
        private WebBrowser _webBrowser;
        private string _html;
        private readonly IFileUtilities _fileUtilities;
        private readonly IRenderingOptionsTreeViewService _renderingOptionsTreeViewService;

        public ViewerService(IApplicationStorageService applicationStorageService, ISolutionListTreeViewService solutionListTreeViewService, IHtmlRenderer htmlRenderer, IActionService actionService, IFileUtilities fileUtilities, IRenderingOptionsTreeViewService renderingOptionsTreeViewService)
        {
            _applicationStorageService = applicationStorageService;
            _solutionListTreeViewService = solutionListTreeViewService;
            _htmlRenderer = htmlRenderer;
            _fileUtilities = fileUtilities;
            _renderingOptionsTreeViewService = renderingOptionsTreeViewService;

            _solutionListTreeViewService.ActionPresentBlobs += SolutionListTreeViewService_ActionPresentBlobs;
            actionService.ActionSave += ActionService_ActionSave;
        }

        private void ActionService_ActionSave(object sender, ActionSaveEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_html))
            {
                _synchronizationContext.Send(new SendOrPostCallback(o =>
                {
                    try
                    {
                        using (var dialog = new SaveFileDialog())
                        {
                            dialog.Filter = "Html files (.html)|*.html";
                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                var fileName = dialog.FileName;

                                if (!string.IsNullOrWhiteSpace(fileName))
                                {
                                    if (File.Exists(fileName))
                                    {
                                        File.Delete(fileName);
                                    }

                                    _fileUtilities.CreateFile(fileName, (string)o);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }), _html);
            }
        }

        public void SetContext(SynchronizationContext synchronizationContext, WebBrowser webBrowser)
        {
            _webBrowser = webBrowser;
            _synchronizationContext = synchronizationContext;
        }

        private async void SolutionListTreeViewService_ActionPresentBlobs(object sender, ActionPresentBlobsEventArgs e)
        {
            var tasks = new List<Task<SolutionList>>();

            foreach (var blobName in e.BlobNames)
            {
                tasks.Add(_applicationStorageService.GetSolutionList(blobName));
            }

            var solutionLists = await Task.WhenAll(tasks);

            var combinedSolutionList = new SolutionList();

            //Todo: log instances that are not of type Solution
            combinedSolutionList.AddRange(solutionLists.SelectMany(s => s).OfType<Solution>());

            _html = await _htmlRenderer.RenderToString(_renderingOptionsTreeViewService.GetRenderProperties(), combinedSolutionList, "Third party dependencies", "Third party dependencies");

            _synchronizationContext.Send(new SendOrPostCallback(o =>
            {
                var htmlToRender = (string)o;
                _webBrowser.DocumentText = htmlToRender;
            }), _html);
        }
    }
}
