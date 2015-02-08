using Gem.Network.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gem.Network.Events
{

    public class CacheEventArgs<TKey, TCached> : EventArgs
    {
        public TKey Key { get; set; }
        public TCached CachedItem { get; set; }
        public long ByteSize { get; set; }
    }

    public class EventAggregator<TKey, TCached> : ICacheEvent<GCache<TKey, TCached>,
                                                     CacheEventArgs<TKey, TCached>>
       where TCached : class
    {

        public void RaiseAddEvent(GCache<TKey, TCached> sender, CacheEventArgs<TKey, TCached> args)
        {
            var handler = OnAddEvent;
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        public void RaiseUsedMemoryChangedEvent(GCache<TKey, TCached> sender, CacheEventArgs<TKey, TCached> args)
        {
            var handler = OnUsedMemoryEvent;
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        public event EventHandler<CacheEventArgs<TKey, TCached>> OnAddEvent;

        public event EventHandler<CacheEventArgs<TKey, TCached>> OnUsedMemoryEvent;

    }

}
