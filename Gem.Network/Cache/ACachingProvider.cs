using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Gem.Network.Cache
{
    /// <summary>
    /// A memory cache wrapper
    /// </summary>
    public abstract class ACachingProvider
    {

        #region Fields

        static readonly object padlock = new object();

        protected MemoryCache cache;      

        #endregion


        #region Constructor

        public ACachingProvider(string cacheName)
        {
            cache = new MemoryCache(cacheName);
        }

        #endregion


        #region Cache Related Protected Methods

        protected virtual void AddItem(string key, object value)
        {
            lock (padlock)
            {
                cache.Add(key, value, DateTimeOffset.MaxValue);
            }
        }

        protected virtual void UpdateItem(string key, object value)
        {
            lock (padlock)
            {
                cache.Set(key, value, DateTimeOffset.MaxValue);
            }
        }

        protected virtual void RemoveItem(string key)
        {
            lock (padlock)
            {
                cache.Remove(key);
            }
        }

        protected virtual object GetItem(string key, bool remove)
        {
            lock (padlock)
            {
                var res = cache[key];

                if (res != null)
                {
                    if (remove == true)
                        cache.Remove(key);
                }
                else
                {
                    WriteToLog("CachingProvider-GetItem: Doesn't contain key: {0}", key);
                }

                return res;
            }
        }

        #endregion


        #region Logs

        protected void WriteToLog(string text, params object[] args)
        {
            //Diagnostics.Logger.LogHelper.Error(text, args);
        }

        #endregion
 
    }
}
