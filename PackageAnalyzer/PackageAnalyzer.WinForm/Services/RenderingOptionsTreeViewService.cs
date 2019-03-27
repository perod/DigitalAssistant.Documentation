using PackageAnalyzer.Core.Model;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PackageAnalyzer.WinForm.Services
{
    public class RenderingOptionsTreeViewService : IRenderingOptionsTreeViewService
    {
        private SynchronizationContext _synchronizationContext;
        private TreeView _treeView;
        
        public Task CreateTree(SynchronizationContext synchronizationContext, TreeView treeView)
        {
            _synchronizationContext = synchronizationContext;
            _treeView = treeView;

            synchronizationContext.Send(new SendOrPostCallback(o => {
                BuildTree();
                _treeView.AfterCheck += TreeViewAfterCheck;
            }), string.Empty);

            return Task.CompletedTask;
        }

        private void TreeViewAfterCheck(object sender, TreeViewEventArgs e)
        {
            if(e.Node.Nodes.Count > 0)
            {
                foreach(TreeNode node in e.Node.Nodes)
                {
                    node.Checked = e.Node.Checked;
                }
            }
        }

        private void BuildTree()
        {
            var properties = RenderProperties.Default;
            AddNode(_treeView.Nodes, typeof(RenderProperties), RenderProperties.Default);
        }
        
        public RenderProperties GetRenderProperties()
        {
            var renderProperties = new RenderProperties();
            object r = renderProperties;
            ReadNode(_treeView.Nodes, ref r);
            return renderProperties;
        }

        private void AddNode(TreeNodeCollection parentNodeCollection, System.Type type, object properties)
        {
            foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = properties != null ? propertyInfo.GetValue(properties) : null;

                var node = parentNodeCollection.Add(propertyInfo.Name);
                node.Tag = propertyInfo;
                
                if (propertyInfo.PropertyType != typeof(System.Boolean))
                {
                    AddNode(node.Nodes, propertyInfo.PropertyType, value);
                }
                else
                {
                    if (properties != null)
                    {
                        node.Checked = (bool)value;
                    }
                }

                node.Expand();
            }
        }

        private void ReadNode(TreeNodeCollection treeNodeCollection, ref object properties)
        {
            foreach (TreeNode node in treeNodeCollection)
            {
                var propertyInfo = node.Tag as PropertyInfo;

                if (propertyInfo.PropertyType == typeof(bool))
                {
                    propertyInfo.SetValue(properties, node.Checked);
                }
                else
                {
                    if (node.Nodes != null)
                    {
                        var childProperties = propertyInfo.GetValue(properties);
                        if (childProperties == null)
                        {
                            childProperties = Activator.CreateInstance(propertyInfo.PropertyType);
                            propertyInfo.SetValue(properties, childProperties);
                        }

                        ReadNode(node.Nodes, ref childProperties);
                    }
                }
            }
        }
    }
}
