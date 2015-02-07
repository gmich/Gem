using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Cache
{

    public class Cache<TKey, TCached> : IDisposable
        where TCached : class
    {
        private Dictionary<TKey,TCached> _cache;
        private readonly int capacity;
        private bool isDisposed;
            
        public Cache(int capacity,IEqualityComparer<TKey> keyEquality)
        {
            this.isDisposed = false;
            this.capacity = capacity;

            _cache = new Dictionary<TKey, TCached>(capacity, keyEquality);
        }

        public void Add(TKey tkey,TCached tcache)
        {
            Guard.That(_cache).IsTrue(x =>(x.Count ==capacity),"Cache is full");

            _cache.Add(tkey,tcache);
        }

        public void FreeResources(int count)
        {
            _cache = new Dictionary<TKey, TCached>(capacity);
        }

        public TCached TryLookup(Func<TKey> tGenerator, out bool result)
        {

            if (_cache.ContainsKey(tGenerator()))
            {
                result = true;
                return _cache[tGenerator()];
            }
         
            result = false;
            return null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                _cache = null;
                isDisposed = true;
            }
        }  

        #endregion

    }
}
