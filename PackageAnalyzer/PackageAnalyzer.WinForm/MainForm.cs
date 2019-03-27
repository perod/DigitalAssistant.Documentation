using PackageAnalyzer.WinForm.Services;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageAnalyzer.WinForm
{
    public partial class MainForm : Form
    {
        private IMainFormService _mainFormService;
        private readonly SynchronizationContext _synchronizationContext;

        public MainForm()
        {
            InitializeComponent();
            _synchronizationContext = SynchronizationContext.Current;
        }

        public async Task InitializeAsync()
        {
            _mainFormService = Ioc.Container.GetInstance<IMainFormService>();
            _mainFormService.InitializeService(_synchronizationContext, _viewerAndOptionsSplitContainer);

            await _solutionListTreeViewUserControl.InitializeAsync(_synchronizationContext);
            await _areaTagUserControl.InitializeAsync(_synchronizationContext);
            await _actionUserControl.InitializeAsync();
            await _viewerUserControl.InitializeAsync(_synchronizationContext);
            await _renderOptionsTreeViewUserControl.InitializeAsync(_synchronizationContext);
        }
    }
}