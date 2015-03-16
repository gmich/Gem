using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Cache
{
    public class CacheProvider : ACachingProvider, IGlobalCachingProvider
    {

        #region Singleton

        private readonly string cacheTag;

        protected CacheProvider(string cacheTag)
            : base(cacheTag)
        {
            this.cacheTag = cacheTag;
        }


        public static CacheProvider Cache
        {
            get
            {
                return Nested.instance;
            }
        }

        internal class Nested
        {
            static Nested()
            {
            }
            internal static readonly CacheProvider instance = new CacheProvider("UserDetails");
        }

        #endregion


        #region ICachingProvider

        public new void AddItem(string key, object value)
        {
            base.AddItem(key, value);
        }

        public new void UpdateItem(string key, object value)
        {
            base.UpdateItem(key, value);
        }

        public object GetItem(string key)
        {
            return base.GetItem(key, true);
        }

        public void RemoveItem(string key)
        {
            base.RemoveItem(key);
        }

        public new object GetItem(string key, bool remove)
        {
            return base.GetItem(key, remove);
        }

        #endregion

    } 
}
