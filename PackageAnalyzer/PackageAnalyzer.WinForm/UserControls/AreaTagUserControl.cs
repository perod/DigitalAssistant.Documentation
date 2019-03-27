using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using PackageAnalyzer.Core.Model;
using System.Threading;
using PackageAnalyzer.WinForm.Services;

namespace PackageAnalyzer.WinForm.UserControls
{
    public partial class AreaTagUserControl : UserControl
    {
        private IAreaTagCheckedListBoxService _areaTagCheckedListBoxService;

        public AreaTagUserControl()
        {
            InitializeComponent();
        }

        public Task InitializeAsync(SynchronizationContext synchronizationContext)
        {
            _areaTagCheckedListBoxService = Ioc.Container.GetInstance<IAreaTagCheckedListBoxService>();
            _areaTagCheckedListBoxService.FillAreaTagCheckedListBox(synchronizationContext, _cbAreaTags);
            _areaTagCheckedListBoxService.SetupEvents(_cbAreaTags);
            return Task.CompletedTask;
        }
    }
}
