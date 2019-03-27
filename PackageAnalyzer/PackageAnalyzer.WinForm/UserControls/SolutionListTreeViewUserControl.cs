using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using PackageAnalyzer.WinForm.Services;
using PackageAnalyzer.Core.Model;
using System.Threading;

namespace PackageAnalyzer.WinForm.UserControls
{
    public partial class SolutionListTreeViewUserControl : UserControl
    {
        private ISolutionListTreeViewService _solutionListTreeViewService;

        public SolutionListTreeViewUserControl()
        {
            InitializeComponent();
        }

        public async Task InitializeAsync(SynchronizationContext synchronizationContext)
        {
            _solutionListTreeViewService = Ioc.Container.GetInstance<ISolutionListTreeViewService>();
            await _solutionListTreeViewService.FillSolutionListTreeView(synchronizationContext, _tree);
        }
    }
}
