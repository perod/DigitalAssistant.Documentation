using PackageAnalyzer.WinForm.Events;
using System.Windows.Forms;

namespace PackageAnalyzer.WinForm.Services
{
    public interface IActionService
    {
        void SetupEvents(Button runBtn, Button refreshBtn, Button saveBtn, Button toggleRenderingOptionsBtn);
        event ActionRunEventHandler ActionRun;
        event ActionRefreshEventHandler ActionRefresh;
        event ActionSaveEventHandler ActionSave;
        event ActionToggleRenderingOptionsEventHandler ActionToggleRenderingOptions;
    }
}
