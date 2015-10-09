using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Gem.IDE.Modules.ProjectExplorer.ProjectTree
{
    public interface ITreeItem
    {
        string DisplayName { get; set; }

        void Configure(TreeView view);

        IEnumerable<ITreeItem> Children { get; set; }
    }

    public class TreeParent : ITreeItem
    {
        public TreeParent(ITreeItem[] items)
        {
        }

        public IEnumerable<ITreeItem> Children
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string DisplayName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Configure(TreeView view)
        {
            throw new NotImplementedException();
        }
    }

    public class TreeItem : ITreeItem
    {
        public TreeItem()
        {
            var i = new TreeView();
            var b = new TreeViewItem();
            var contextMenu = new ContextMenu();
        }

        public IEnumerable<ITreeItem> Children
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string DisplayName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Configure(TreeView view)
        {
            throw new NotImplementedException();
        }

        public ContextMenu DefaultRightClickMenu()
        {
            var contextMenu = new ContextMenu();
            var openItem = new MenuItem();
            openItem.Header = "Open";
            contextMenu.Items.Add(openItem);

            return contextMenu;
        }
    }
}
