using Gem.Network.Utilities;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Network.Repositories
{
    /// <summary>
    /// A repository for storing objects and accessing them by id using the flyweight pattern
    /// </summary>
    /// <typeparam name="Titem">The object to store</typeparam>
    /// <typeparam name="Tid">It's id</typeparam>
    public class FlyweightRepository<Titem, Tid> : IDataProvider<Titem, Tid>
        where Titem : class
    {

        #region Ctor

        private readonly Dictionary<Tid,Titem> items;

        public FlyweightRepository()
        {
            items = new Dictionary<Tid,Titem>();
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
            return items.ContainsKey(key);
        }

        #region Get

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

        #endregion

        #region Update

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

        #endregion

        #region Delete

        public bool Delete(Tid id)
        {
            if(items.ContainsKey(id))
            {
                return items.Remove(id);
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

        #endregion

    }
}
