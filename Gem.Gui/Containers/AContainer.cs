using System;
using System.Collections.Generic;

namespace Gem.Gui.Containers
{
    public class AContainer<TContainerItem>
    {
        private readonly Dictionary<string, TContainerItem> items = new Dictionary<string, TContainerItem>();

        public bool Add(string id, Func<TContainerItem> itemRetriever)
        {
            if(items.ContainsKey(id))
            {
                return false;
            }
            items.Add(id, itemRetriever());
            return true;
        }

        public bool Has(string id)
        {
            return items.ContainsKey(id);
        }

        public bool Remove(string id)
        {
            return items.Remove(id);

        }
        public bool Add(string id, TContainerItem item)
        {
            if (items.ContainsKey(id))
            {
                return false;
            }
            items.Add(id, item);
            return true;
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
