using System.Threading;
using System.Windows.Forms;

namespace PackageAnalyzer.WinForm.Services
{
    public interface IViewerService
    {
        void SetContext(SynchronizationContext synchronizationContext, WebBrowser _webBrowser);
    }
}
