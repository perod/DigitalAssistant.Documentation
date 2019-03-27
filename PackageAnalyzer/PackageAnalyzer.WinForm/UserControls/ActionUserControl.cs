using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using PackageAnalyzer.WinForm.Services;

namespace PackageAnalyzer.WinForm.UserControls
{
    public partial class ActionUserControl : UserControl
    {
        private IActionService _actionService;

        public ActionUserControl()
        {
            InitializeComponent();
        }

        public Task InitializeAsync()
        {
            _actionService = Ioc.Container.GetInstance<IActionService>();
            _actionService.SetupEvents(_bRun, _bRefresh, _bSaveToFile, _bToggleRenderingOptions);
            return Task.CompletedTask;
        }
    }
}
