using System;
using System.Collections.Generic;

namespace Gem.Gui.Containers
{
    public class AContainer<TContainerItem>
    {
        private readonly Dictionary<string, TContainerItem> items = new Dictionary<string, TContainerItem>();

        public void Add(string id, Func<TContainerItem> itemRetriever)
        {
            items.Add(id, itemRetriever());
        }

        public bool Remove(string id)
        {
            return items.Remove(id);

        }
        public void Add(string id, TContainerItem item)
        {
            items.Add(id, item);
        }

        public TContainerItem this[string id]
        {
            get
            {
                return items.ContainsKey(id) ?
                       items[id] : default(TContainerItem);
            }
        }
    }
}
