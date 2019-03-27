using PackageAnalyzer.Core.Model;
using System;
using System.Collections.Generic;

namespace PackageAnalyzer.WinForm.Events
{
    public delegate void AreaTagCheckedListBoxCheckChangedEventHandler(object sender, AreaTagCheckedListBoxCheckChangedEventArgs e);

    public class AreaTagCheckedListBoxCheckChangedEventArgs : EventArgs
    {
        public List<SolutionAreaTag> SolutionAreaTags { get; private set; }

        public AreaTagCheckedListBoxCheckChangedEventArgs(List<SolutionAreaTag> solutionAreaTags)
        {
            SolutionAreaTags = solutionAreaTags;
        }
    }
}
