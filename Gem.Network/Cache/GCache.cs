#region Usings

using Gem.Network.Cache.Events;
using Seterlund.CodeGuard;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Gem.Network.Cache
{

    /// <summary>
    /// A cache for storing objects 
    /// </summary>
    /// <typeparam name="TKey">The lookups key</typeparam>
    /// <typeparam name="TCached">The cached object</typeparam>
    public class GCache<TKey, TCached> : IDisposable
        where TCached : class
    {

        #region Fields

        private readonly TCached NotFound = default(TCached);

        /// <summary>
        /// The cache's buffer in bytes
        /// </summary>
        protected readonly long buffer;

        protected readonly MemoryCalculator memoryCalculator;

        private long _memoryUsed;

        protected long MemoryUsed
        {
            get
            {
                return _memoryUsed;
            }
            set
            {
                _memoryUsed = value;
                Events.RaiseUsedMemoryChangedEvent(this, new CacheEventArgs<TKey, TCached>
                {
                    ByteSize = _memoryUsed
                });
            }
        }


        protected ConcurrentDictionary<TKey, CacheEntry> _cache;

        protected bool isDisposed;

        #endregion

        #region Public Events

        public EventAggregator<TKey, TCached> Events
        {
            get;
            set;
        }

        #endregion

        #region Construct / Dispose

        /// <summary>
        /// Instantiate cache
        /// </summary>
        /// <param name="capacity">The buffers capacity</param>
        /// <param name="keyEquality">How a cache lookup is performed</param>
        public GCache(long capacity, IEqualityComparer<TKey> keyEquality)
        {
            _cache = new ConcurrentDictionary<TKey, CacheEntry>(keyEquality);
            memoryCalculator = new MemoryCalculator();
            this.isDisposed = false;
            this.buffer = capacity;
            Events = new EventAggregator<TKey, TCached>();
        }

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

        #region Public Methods

        /// <summary>
        /// Add an item to the cache by key
        /// </summary>
        /// <param name="tkey">The key</param>
        /// <param name="tcache">The object</param>
        public void Add(TKey tkey, TCached tcache)
        {
            long additionalSize = (memoryCalculator.GetSizeInBytes(tkey) + memoryCalculator.GetSizeInBytes(tcache));
            Guard.That(MemoryUsed + additionalSize, "Object is too big to store in the cache. Consider deallocating")
                 .IsLessThan(buffer);

            if (_cache.ContainsKey(tkey)) return;

            if (_cache.TryAdd(tkey, new CacheEntry(tcache, additionalSize)))
            {
                MemoryUsed += additionalSize;
                Events.RaiseAddEvent(this, new CacheEventArgs<TKey, TCached>
                {
                    Key = tkey,
                    CachedItem = tcache,
                    ByteSize = additionalSize
                });
                ManageSize();
            }
        }

        public TCached Lookup(Func<TKey, TKey, bool> sequenceEqual, TKey keyLookup)
        {
            foreach (var cache in _cache)
            {
                if (sequenceEqual(cache.Key, keyLookup))
                {
                    //put the most looked up entries at the end by removing and adding
                    CacheEntry entry;
                    if (_cache.TryRemove(cache.Key, out entry))
                    {
                        _cache.TryAdd(cache.Key, entry);
                    }
                    return _cache[keyLookup].CachedEntry;
                }
            }
            return NotFound;
        }

        /// <summary>
        /// Performs a cache lookup by key.
        /// </summary>
        /// <param name="keyLookup">The key </param>
        /// <returns>The value or the default(value) if the key is not found</returns>
        public TCached Lookup(TKey keyLookup)
        {
            if (_cache.ContainsKey(keyLookup))
            {
                //put the most looked up entries at the end by removing and adding
                CacheEntry entry;
                if (_cache.TryRemove(keyLookup, out entry))
                {
                    _cache.TryAdd(keyLookup, entry);
                }
                return _cache[keyLookup].CachedEntry;
            }
            return NotFound;
        }

        #endregion

        #region Memory Management

        /// <summary>
        /// If the buffer is almost full, free resources
        /// </summary>
        protected virtual void ManageSize()
        {
            if (MemoryUsed > ((buffer * 2) / 3))
            {
                FreeResources(MemoryUsed / 4);
            }
        }

        private bool TryDistinct()
        {
            _cache.Distinct();

            var cachedMemory = _cache.Select(x => x.Value.ByteSize).ToList();
            long currentMemoryUsed = 0;
            cachedMemory.ForEach(x => currentMemoryUsed += x);

            if (currentMemoryUsed < MemoryUsed)
            {
                MemoryUsed = currentMemoryUsed;
                return true;
            }
            return false;
        }

        protected void FreeResources(long memoryToFree)
        {
            Guard.That(memoryToFree < MemoryUsed,
             "Invalid Operation. The specified memory to free is more than the available memory");

            long currentMemoryUsed = 0;

            while (currentMemoryUsed < memoryToFree)
            {
                if (_cache.IsEmpty) return;
                var firstKey = _cache.Select(x => x.Key).First();
                CacheEntry entry;
                if (_cache.TryRemove(firstKey, out entry))
                {
                    currentMemoryUsed += entry.ByteSize;
                    MemoryUsed -= entry.ByteSize;
                }
            }
        }

        /// <summary>
        /// Calculates an object's size in bytes
        /// </summary>
        protected class MemoryCalculator
        {
            private Stream ms;
            private BinaryFormatter formatter;

            public MemoryCalculator()
            {
                ms = new MemoryStream();
                formatter = new BinaryFormatter();
            }

            public long GetSizeInBytes(object obj)
            {
                ms = new MemoryStream();
                formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.Length;
            }
        }

        /// <summary>
        /// An entry in the cache with its size in bytes
        /// </summary>
        protected class CacheEntry
        {
            public CacheEntry(TCached cachedEntry, long byteSize)
            {
                this.CachedEntry = cachedEntry;
                this.ByteSize = byteSize;
            }

            public TCached CachedEntry { get; set; }
            public long ByteSize { get; set; }
        }

        #endregion

    }
}
