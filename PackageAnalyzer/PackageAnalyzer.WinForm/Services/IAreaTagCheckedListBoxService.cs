using System.Threading;
using System.Windows.Forms;
using PackageAnalyzer.WinForm.Events;

namespace PackageAnalyzer.WinForm.Services
{
    public interface IAreaTagCheckedListBoxService
    {
        void FillAreaTagCheckedListBox(SynchronizationContext context, CheckedListBox checkedListBox);
        void SetupEvents(CheckedListBox checkedListBox);

        event AreaTagCheckedListBoxCheckChangedEventHandler AreaTagCheckedListBoxCheckChanged;
    }
}
