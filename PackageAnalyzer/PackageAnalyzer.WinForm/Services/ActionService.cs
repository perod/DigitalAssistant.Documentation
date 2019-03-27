using PackageAnalyzer.WinForm.Events;
using System;
using System.Windows.Forms;

namespace PackageAnalyzer.WinForm.Services
{
    public class ActionService : IActionService
    {
        public event ActionRunEventHandler ActionRun;
        public event ActionRefreshEventHandler ActionRefresh;
        public event ActionSaveEventHandler ActionSave;
        public event ActionToggleRenderingOptionsEventHandler ActionToggleRenderingOptions;

        public void SetupEvents(Button runBtn, Button refreshBtn, Button saveBtn, Button toggleRenderingOptionsBtn)
        {
            runBtn.Click += RunBtnClick;
            refreshBtn.Click += RefreshBtnClick;
            saveBtn.Click += SaveBtnClick;
            toggleRenderingOptionsBtn.Click += ToggleRenderingOptionsBtnClick;
        }

        private void ToggleRenderingOptionsBtnClick(object sender, EventArgs e)
        {
            ActionToggleRenderingOptions?.Invoke(sender, new ActionToggleRenderingOptionsEventArgs());
        }

        private void SaveBtnClick(object sender, EventArgs e)
        {
            ActionSave?.Invoke(sender, new ActionSaveEventArgs());
        }

        private void RefreshBtnClick(object sender, EventArgs e)
        {
            ActionRefresh?.Invoke(sender, new ActionRefreshEventArgs());
        }

        private void RunBtnClick(object sender, EventArgs e)
        {
            ActionRun?.Invoke(sender, new ActionRunEventArgs());
        }
    }
}
