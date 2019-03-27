using PackageAnalyzer.WinForm.Events;
using System.Windows.Forms;
using System.Threading;

namespace PackageAnalyzer.WinForm.Services
{
    public class MainFormService : IMainFormService
    {
        private readonly IActionService _actionService;

        private SynchronizationContext _synchronizationContext;
        private SplitContainer _toggleRenderingOptionsSplitContainer;

        public MainFormService(IActionService actionService)
        {
            _actionService = actionService;
            _actionService.ActionToggleRenderingOptions += ActionServiceActionToggleRenderingOptions;
        }

        public void InitializeService(SynchronizationContext synchronizationContext, SplitContainer toggleRenderingOptionsSplitContainer)
        {
            _synchronizationContext = synchronizationContext;
            _toggleRenderingOptionsSplitContainer = toggleRenderingOptionsSplitContainer;
        }

        private void ActionServiceActionToggleRenderingOptions(object sender, ActionToggleRenderingOptionsEventArgs e)
        {
            _toggleRenderingOptionsSplitContainer.Panel2Collapsed = !_toggleRenderingOptionsSplitContainer.Panel2Collapsed;
        }
    }
}
