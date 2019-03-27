using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageAnalyzer.WinForm
{
    public class MainFormStarter
    {
        private readonly MainForm _mainForm;
        public MainFormStarter()
        {
            _mainForm = new MainForm();
            _mainForm.FormClosed += MainFormFormClosed;
        }

        public async Task StartAsync()
        {
            await _mainForm.InitializeAsync();
            _mainForm.Show();
        }

        public event EventHandler<EventArgs> ExitRequested;

        void MainFormFormClosed(object sender, FormClosedEventArgs e)
        {
            OnExitRequested(EventArgs.Empty);
        }

        protected virtual void OnExitRequested(EventArgs e)
        {
            ExitRequested?.Invoke(this, e);
        }
    }
}
