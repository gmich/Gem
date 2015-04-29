using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Factories
{
    public class AContainer<TContainerItem>
    {
        private readonly Dictionary<string, TContainerItem> items = new Dictionary<string, TContainerItem>();
        
        public void Add(string id, Func<TContainerItem> itemRetriever)
        {
            items.Add(id, itemRetriever());
        }

        //TODO: implement remove;

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
