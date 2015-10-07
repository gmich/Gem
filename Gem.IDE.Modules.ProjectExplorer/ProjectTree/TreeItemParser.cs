using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gem.IDE.Modules.ProjectExplorer.ProjectTree
{
    class TreeItemParser
    {
        private readonly IEnumerable<XToken> tokens;
        public void Parse(string xmlDoc)
        {
            XDocument xml = XDocument.Parse(xmlDoc);
            var res =
                (from e in xml.Root.Elements("GemTree")
                 let r = e.Element("Region")
                 select tokens
                       .Where(token => token.Name == r.Name)
                       .Select(token => new
                       {
                           X = token.FoundCallback(r.Name.ToString())
                       }))
                 .ToList();
        }
    }

    public class TreeItemBuilder
    {
        private readonly Stack<Group> cachedNodes = new Stack<Group>();
        private Func<ITreeItem, ITreeItem> decorator = node => node;

        public TreeItemBuilder()
        {
            cachedNodes.Push(new Group((node, group) => { }));
        }

        private class Group
        {
            public Group(Action<IEnumerable<ITreeItem>, Group> groupEndCallback)
            {
                GroupEndCallback = groupEndCallback;
            }

            public List<ITreeItem> Nodes { get; } = new List<ITreeItem>();

            public Action<IEnumerable<ITreeItem>, Group> GroupEndCallback { get; }
        }

        public TreeItemBuilder Leaf(ITreeItem node)
        {
            Add(decorator(node));

            return this;
        }

        private void Add(ITreeItem node)
        {
            var group = cachedNodes.Pop();

            group.Nodes.Add(node);
            cachedNodes.Push(group);
        }

        public TreeItemBuilder End
        {
            get
            {
                var group = cachedNodes.Pop();
                var previousGroup = cachedNodes.Pop();
                cachedNodes.Push(previousGroup);
                group.GroupEndCallback(group.Nodes, previousGroup);
                return this;
            }
        }

        public TreeItemBuilder Decorate(Func<ITreeItem, ITreeItem> newDecorator)
        {
            decorator = node =>
            {
                decorator = n => n;
                return newDecorator(node);
            };
            return this;
        }

        public TreeItemBuilder Parent
        {
            get
            {
                var group = new Group((nodes, grp) =>
                {
                    grp.Nodes.Add(decorator(new TreeParent(nodes.ToArray())));
                });
                cachedNodes.Push(group);
                return this;
            }
        }


        public ITreeItem Tree
        {
            get
            {
                if (cachedNodes.Count != 1)
                {
                    throw new Exception("Root should contain only one node");
                }
                return cachedNodes.Pop().Nodes[0];
            }
        }

    }
    internal class XToken
    {
        public string Name { get; }
        public Func<string, ITreeItem> FoundCallback { get; }

        public XToken(string name, Func<string, ITreeItem> foundCallback)
        {
            Name = name;
            FoundCallback = foundCallback;
        }
    }
}