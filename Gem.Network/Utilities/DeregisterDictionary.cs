using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Network.Utilities
{
    /// <summary>
    /// A disposable class for that removes entries from a generic dictionary
    /// </summary>
    /// <typeparam name="TKey">The dictionary's key</typeparam>
    /// <typeparam name="TValue">Te dictionary's value</typeparam>
    public class DeregisterDictionary<TKey, TValue> : IDisposable
    {
        private Dictionary<TKey, TValue> registered;
        private TValue current;

        public DeregisterDictionary(Dictionary<TKey, TValue> registered, TValue current)
        {
            this.registered = registered;
            this.current = current;
        }

        public void Dispose()
        {
            foreach (var item in registered.Where(kvp => kvp.Value.Equals(current)).ToList())
            {
                registered.Remove(item.Key);
            }
        }
    }
}
