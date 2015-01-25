using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gem.Network.Utilities
{
    //public class Deregister<T> : IDisposable
    //{
    //    private List<T> registered;
    //    private T current;

    //    public Deregister(List<T> registered, T current)
    //    {
    //        this.registered = registered;
    //        this.current = current;
    //    }

    //    public void Dispose()
    //    {
    //        if (registered.Contains(current))
    //            registered.Remove(current);
    //    }
    //}

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
