using System.Threading;
using System.Windows.Forms;

namespace PackageAnalyzer.WinForm.Services
{
    public interface IMainFormService
    {
        void InitializeService(SynchronizationContext synchronizationContext, SplitContainer toggleRenderingOptionsSplitContainer);
    }
}
