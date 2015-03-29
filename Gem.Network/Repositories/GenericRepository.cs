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

        #region Fields

        private readonly List<Titem> items;

        private readonly Func<Titem, Tid> idProperty;

        #endregion

        #region Ctor

        /// <summary>
        /// Initialize a new instance of GenericRepository
        /// </summary>
        /// <param name="idProperty">Specify how the id is accessed by delegate</param>
        public GenericRepository(Func<Titem, Tid> idProperty)
        {
            this.idProperty = idProperty;
            items = new List<Titem>();
        }

        #endregion

        #region Properties

        public int TotalElements
        {
            get
            {
                return items.Count;
            }
        }

        #endregion

        public bool HasKey(Tid key)
        {
            return items.Any(x => idProperty(x).Equals(key));
        }

        #region Get

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

        #endregion

        #region Update

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

        #endregion

        #region Delete

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

        #endregion

        #region Add

        /// <summary>
        /// Add and return the entry as IDisposable.
        /// By disposing, the entry is removed
        /// </summary>
        /// <param name="id">The objects' id</param>
        /// <param name="item">The object to store</param>
        /// <returns>The entry's disposable</returns>
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

        #endregion

        #region Disposable Object

        internal class DeregisterDisposable<T> : IDisposable
        {
            private readonly List<T> registered;
            private readonly T current;

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

        #endregion

    }
}
