using Gem.Network.Utilities;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Repositories
{
    public class FlyweightRepository<Titem, Tid> : IDataProvider<Titem, Tid>
        where Titem : class
    {
        private Dictionary<Tid,Titem> items;

        public FlyweightRepository()
        {
            items = new Dictionary<Tid,Titem>();
        }

        public int TotalElements
        {
            get
            {
                return items.Count;
            }
        }

        public bool HasKey(Tid key)
        {
            return items.ContainsKey(key);
        }

        public List<Titem> GetAll()
        {
            return items.Select(x=>x.Value).ToList();
        }

        public Titem GetById(Tid id)
        {
            if(items.ContainsKey(id))
            {
                return items[id];
            }
            return null;
        }

        public Titem Get(Func<Titem, bool> expression)
        {
            return items.Where(x => expression(x.Value)).Select(x => x.Value).FirstOrDefault();
        }

        public bool Update(Tid id, Titem item)
        {
            Guard.That(item).IsNotDefault();
            
            if (items.ContainsKey(id))
            {
                items[id] = item;
                return true;
            }
            return false;
        }

        public bool Delete(Tid id)
        {
            if(items.ContainsKey(id))
            {
                return items.Remove(id);
            }
            return false;
        }

        public IDisposable Add(Tid id, Titem item)
        {          
            Guard.That(item).IsNotNull();
            if (!items.ContainsKey(id))
            {
                items.Add(id,item);
                return new DeregisterDictionary<Tid,Titem>(items, item);   
            }
            return null;
        }

     
    }
}
