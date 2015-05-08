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
    public sealed class DeregisterDictionary<TKey, TValue> : IDisposable
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool isDisposed = false;
        private void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                foreach (var item in registered.Where(kvp => kvp.Value.Equals(current)).ToList())
                {
                    registered.Remove(item.Key);
                }
                isDisposed = true;
            }
        }

    }
}
