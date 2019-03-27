using PackageAnalyzer.Core.Model;
using PackageAnalyzer.WinForm.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PackageAnalyzer.WinForm.Services
{
    public class AreaTagCheckedListBoxService : IAreaTagCheckedListBoxService
    {
        private CheckedListBox _checkedListBox;

        public event AreaTagCheckedListBoxCheckChangedEventHandler AreaTagCheckedListBoxCheckChanged;

        public void FillAreaTagCheckedListBox(SynchronizationContext context, CheckedListBox checkedListBox)
        {
            _checkedListBox = checkedListBox;

            context.Send(new SendOrPostCallback(o =>
            {
                var values = Enum.GetValues(typeof(SolutionAreaTag)).Cast<SolutionAreaTag>();
                _checkedListBox.Items.AddRange(values.Select(v => v.ToString()).ToArray());
            }), string.Empty);
        }


        public void SetupEvents(CheckedListBox checkedListBox)
        {
            checkedListBox.ItemCheck += CheckedListBoxItemCheck;
        }

        private void CheckedListBoxItemCheck(object sender, ItemCheckEventArgs e)
        {
            var index = e.Index;
            var checkedSolutionAreaTags = new List<SolutionAreaTag>();

            for (var i = 0; i < _checkedListBox.Items.Count; i++)
            {
                
                CheckState state = i == index
                    ? e.NewValue
                    : _checkedListBox.GetItemChecked(i) == true ? CheckState.Checked : CheckState.Unchecked;

                if (state == CheckState.Checked)
                {
                    var sSolutionAreaTag = _checkedListBox.Items[i].ToString();
                    SolutionAreaTag result;
                    if (Enum.TryParse(sSolutionAreaTag, out result))
                    {
                        checkedSolutionAreaTags.Add(result);
                    }
                }
            }
            
            AreaTagCheckedListBoxCheckChanged?.Invoke(sender, new AreaTagCheckedListBoxCheckChangedEventArgs(checkedSolutionAreaTags));

        }
    }
}
