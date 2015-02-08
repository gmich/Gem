using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Cache
{
    /// <summary>
    /// The base interface for memory caching
    /// </summary>
    public interface IGlobalCachingProvider
    {
        void AddItem(string key, object value);

        void UpdateItem(string key, object value);

        void RemoveItem(string key);

        object GetItem(string key);
    } 
}
