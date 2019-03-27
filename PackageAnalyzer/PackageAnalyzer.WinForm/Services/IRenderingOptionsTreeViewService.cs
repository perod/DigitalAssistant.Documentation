using PackageAnalyzer.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageAnalyzer.WinForm.Services
{
    public interface IRenderingOptionsTreeViewService
    {
        Task CreateTree(SynchronizationContext synchronizationContext, TreeView treeView);
        RenderProperties GetRenderProperties();
    }
}
