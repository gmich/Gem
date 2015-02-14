using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Network.Repositories
{
    public class GenericRepository<Titem, Tid> : IDataProvider<Titem, Tid>
        where Titem : class
        where Tid : struct
    {
        private List<Titem> items;
        private Func<Titem, Tid> idProperty;

        public GenericRepository(Func<Titem, Tid> idProperty)
        {
            this.idProperty = idProperty;
            items = new List<Titem>();
        }

        public Titem GetById(Tid id)
        {
            return items.Where(x => idProperty(x).Equals(id)).FirstOrDefault();
        }

        public Titem Get(Func<Titem, bool> expression)
        {
            return items.Where(x => expression(x)).Select(x => x).FirstOrDefault();
        }

        public bool Update(Titem item)
        {
            Guard.That(item).IsNotDefault();

            var itemToUpdate = items.Find(x => x.Equals(item));

            if (itemToUpdate != null)
            {
                itemToUpdate = item;
                return true;
            }
            return false;
        }

        public bool Delete(Tid id)
        {
            var entryToRemove = GetById(id);
            if (entryToRemove != null)
            {
                items.Remove(entryToRemove);
                return true;
            }
            return false;
        }

        public bool Add(Titem item)
        {
            Guard.That(item).IsNotDefault();

            if (!items.Contains(item))
            {
                items.Add(item);
                return true;
            }
            return false;
        }
    }
}
