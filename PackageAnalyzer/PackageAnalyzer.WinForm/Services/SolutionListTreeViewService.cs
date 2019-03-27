using PackageAnalyzer.Core.Model;
using PackageAnalyzer.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using PackageAnalyzer.WinForm.Events;
using System.Threading.Tasks;

namespace PackageAnalyzer.WinForm.Services
{
    public class SolutionListTreeViewService : ISolutionListTreeViewService
    {
        private readonly IAreaTagCheckedListBoxService _areaTagCheckedListBoxService;
        private readonly IActionService _actionService;
        private readonly IApplicationStorageService _applicationStorageService;

        private TreeView _treeView;
        private SynchronizationContext _synchronizationContext;

        public event ActionPresentBlobsEventHandler ActionPresentBlobs;

        public SolutionListTreeViewService(IApplicationConfiguration applicationConfiguration, IApplicationStorageService applicationStorageService, IAreaTagCheckedListBoxService areaTagCheckedListBoxService, IActionService actionService)
        {
            _applicationStorageService = applicationStorageService;
            _areaTagCheckedListBoxService = areaTagCheckedListBoxService;
            _actionService = actionService;
            _areaTagCheckedListBoxService.AreaTagCheckedListBoxCheckChanged += AreaTagCheckedListBoxService_AreaTagCheckedListBoxCheckChanged;
            _actionService.ActionRun += ActionService_ActionRun;
            _actionService.ActionRefresh += ActionService_ActionRefresh;
        }

        private async void ActionService_ActionRefresh(object sender, ActionRefreshEventArgs e)
        {
            await FillSolutionListTreeView(_synchronizationContext, _treeView);
        }

        private void ActionService_ActionRun(object sender, Events.ActionRunEventArgs e)
        {
            var blobs = new List<string>();
            foreach (TreeNode rootNode in _treeView.Nodes)
            {
                foreach (TreeNode childNode in rootNode.Nodes)
                {
                    if (childNode.Checked)
                    {
                        var storageIdentifier = childNode.Tag as StorageIdentifier;
                        if (storageIdentifier != null)
                        {
                            blobs.Add(storageIdentifier.BlobName);
                        }
                    }
                }
            }

            ActionPresentBlobs?.Invoke(sender, new ActionPresentBlobsEventArgs(blobs));
        }

        private void AreaTagCheckedListBoxService_AreaTagCheckedListBoxCheckChanged(object sender, Events.AreaTagCheckedListBoxCheckChangedEventArgs e)
        {
            var hasFilter = e.SolutionAreaTags.Any();

            foreach (TreeNode rootNode in _treeView.Nodes)
            {
                var first = true;
                foreach (TreeNode childNode in rootNode.Nodes)
                {
                    if (hasFilter && first)
                    {
                        bool hasFlag = false;
                        var storageIdentifier = childNode.Tag as StorageIdentifier;
                        if (storageIdentifier != null)
                        {
                            foreach (var solutionAreaTag in e.SolutionAreaTags)
                            {
                                if (storageIdentifier.AreaTags.HasFlag(solutionAreaTag))
                                {
                                    hasFlag = true;
                                    break;
                                }
                            }
                        }
                        childNode.Checked = hasFlag;
                    }
                    else
                    {
                        childNode.Checked = first;
                    }
                    first = false;
                }
            }
        }

        public async Task FillSolutionListTreeView(SynchronizationContext synchronizationContext, TreeView treeView)
        {
            _treeView = treeView;
            _synchronizationContext = synchronizationContext;
            var storageIdentifiers = await _applicationStorageService.GetSolutionListStorageIdentifiers();

            if (storageIdentifiers?.Any() == true)
            {
                synchronizationContext.Send(new SendOrPostCallback(o =>
                {
                    _treeView.Nodes.Clear();

                    var data = (List<StorageIdentifier>)o;

                    data
                        .GroupBy(s => s.Name)
                        .ToList()
                        .ForEach(g =>
                        {
                            var rootNode = treeView.Nodes.Add(g.Key);
                            var first = true;
                            g
                            .OrderByDescending(s => s.Date)
                            .ThenByDescending(s => s.BuildId)
                            .ToList()
                            .ForEach(s =>
                            {
                                var childNode = rootNode.Nodes.Add(s.Name + "-" + s.Date.ToShortDateString() + "-" + s.BuildId);
                                if (first)
                                {
                                    childNode.Checked = true;
                                    first = false;
                                }

                                childNode.Tag = s;
                            });

                            rootNode.Expand();
                        });
                }), storageIdentifiers);
            }
        }
    }
}
