using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PackageAnalyzer.WinForm.Services;
using System.Threading;

namespace PackageAnalyzer.WinForm.UserControls
{
    public partial class ViewerUserControl : UserControl
    {
        private IViewerService _viewerService;

        public ViewerUserControl()
        {
            InitializeComponent();
        }

        public Task InitializeAsync(SynchronizationContext synchronizationContext)
        {
            _viewerService = Ioc.Container.GetInstance<IViewerService>();
            _viewerService.SetContext(synchronizationContext, _webBrowser);
            return Task.CompletedTask;
        }
    }
}
