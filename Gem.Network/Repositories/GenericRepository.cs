using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gem.Network.Repositories
{
    /// <summary>
    /// Generic class for managing repositories
    /// </summary>
    /// <typeparam name="Titem">The object to manage</typeparam>
    /// <typeparam name="Tid">The object's unique id</typeparam>
    public class GenericRepository<Titem, Tid> : IDataProvider<Titem, Tid>
        where Titem : class
    {
        private List<Titem> items;
        private Func<Titem, Tid> idProperty;

        public GenericRepository(Func<Titem, Tid> idProperty)
        {
            this.idProperty = idProperty;
            items = new List<Titem>();
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
            return items.Any(x => idProperty(x).Equals(key));
        }

        public List<Titem> GetAll()
        {
            return items;
        }

        public Titem GetById(Tid id)
        {
            return items.Where(x => idProperty(x).Equals(id)).FirstOrDefault();
        }

        public Titem Get(Func<Titem, bool> expression)
        {
            return items.Where(x => expression(x)).Select(x => x).FirstOrDefault();
        }

        public bool Update(Tid id, Titem item)
        {
            Guard.That(item).IsNotDefault();

            var itemToUpdate = this.GetById(id);

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

        public IDisposable Add(Tid id,Titem item)
        {          
            Guard.That(item).IsNotNull();

            if (!items.Contains(item))
            {
                items.Add(item);
                return new DeregisterDisposable<Titem>(items, item);   
            }
            return null;
        }

        internal class DeregisterDisposable<T> : IDisposable
        {
            private List<T> registered;
            private T current;

            internal DeregisterDisposable(List<T> registered, T current)
            {
                this.registered = registered;
                this.current = current;
            }

            public void Dispose()
            {
                if (registered.Contains(current))
                    registered.Remove(current);
            }
        }
    }
}
