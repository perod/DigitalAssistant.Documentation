using PackageAnalyzer.Core.Model;
using PackageAnalyzer.WinForm.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageAnalyzer.WinForm.Services
{
    public interface ISolutionListTreeViewService
    {       
        Task FillSolutionListTreeView(SynchronizationContext context, TreeView treeView);
        event ActionPresentBlobsEventHandler ActionPresentBlobs;
    }
}
