using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageAnalyzer.WinForm
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainFormStarter formStarter = new MainFormStarter();
            formStarter.ExitRequested += FormStarterExitRequested;
            var programStartTask = formStarter.StartAsync();

            Application.Run();
        }

        static void FormStarterExitRequested(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private static async void HandleExceptions(Task task)
        {
            try
            {
                //Force this to yield to the caller, so Application.Run will be executing
                await Task.Yield();
                await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Application.Exit();
            }
        }
    }
}
