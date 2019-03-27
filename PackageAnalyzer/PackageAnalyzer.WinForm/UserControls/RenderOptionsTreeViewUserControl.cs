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
    public partial class RenderOptionsTreeViewUserControl : UserControl
    {
        private IRenderingOptionsTreeViewService _renderingOptionsTreeViewService;

        public RenderOptionsTreeViewUserControl()
        {
            InitializeComponent();
        }

        public async Task InitializeAsync(SynchronizationContext synchronizationContext)
        {
            _renderingOptionsTreeViewService = Ioc.Container.GetInstance<IRenderingOptionsTreeViewService>();
            await _renderingOptionsTreeViewService.CreateTree(synchronizationContext, _treeViewRenderingOptions);
        }
    }
}
