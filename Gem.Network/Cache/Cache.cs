#region Usings

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

    public sealed class Cache<TKey, TCached> : IDisposable
        where TCached : class
    {

        #region Fields

        private readonly TCached NotFound = default(TCached);

        private readonly long capacity;

        private readonly MemoryCalculator memoryCalculator;

        private Timer stateTimer;

        private long memoryUsed;

        private ConcurrentDictionary<TKey, CacheEntry> _cache;

        private bool isDisposed;

        #endregion


        #region Properties

        private bool IsBufferFull
        {
            get
            {
                return memoryUsed >= capacity;
            }
        }

        #endregion


        #region Construct / Dispose

        public Cache(long capacity, IEqualityComparer<TKey> keyEquality)
        {
            _cache = new ConcurrentDictionary<TKey, CacheEntry>(keyEquality);
            memoryCalculator = new MemoryCalculator();
            this.isDisposed = false;
            this.capacity = capacity;

            InitializeMemoryManagementThread();
        }

        private void InitializeMemoryManagementThread()
        {
            //Process.GetCurrentProcess().PrivateMemorySize64;
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            TimerCallback tcb = this.ManageSize;

            //invoke every 5 minutes
            stateTimer = new Timer(tcb, autoEvent, 300000, 300000);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                _cache = null;
                stateTimer.Dispose();
                isDisposed = true;
            }
        }

        #endregion


        #region Public Methods

        public void Add(TKey tkey, TCached tcache)
        {
            Guard.That(_cache).IsTrue(x => (x.Count == capacity), "Cache is full");

            memoryUsed += (memoryCalculator.GetSizeInBytes(tkey) + memoryCalculator.GetSizeInBytes(tcache));
            _cache.TryAdd(tkey, new CacheEntry(tcache, memoryUsed));
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

        #endregion


        #region Memory Management

        private void ManageSize(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;

            if (IsBufferFull)
            {
                if (TryDistinct())
                    FreeResources(memoryUsed / 4);
            }
        }

        private bool TryDistinct()
        {
            _cache.Distinct();

            var cachedMemory = _cache.Select(x => x.Value.ByteSize).ToList();
            long currentMemoryUsed = 0;
            cachedMemory.ForEach(x => currentMemoryUsed += x);

            if (currentMemoryUsed < memoryUsed)
            {
                memoryUsed = currentMemoryUsed;
                return true;
            }
            return false;
        }

        private void FreeResources(long memoryToFree)
        {
            Guard.That(memoryToFree < memoryUsed,
             "Invalid Operation. The specified memory to free is more than the available memory");

            long currentMemoryUsed = 0;
            int entriesToRemove = 0;

            //TODO: find another way to deallocate, without changing types
            _cache.ToList().ForEach(x =>
            {
                if (currentMemoryUsed >= memoryToFree) return;
                currentMemoryUsed += x.Value.ByteSize;
                entriesToRemove++;
            });
            _cache.ToList().RemoveRange(0, entriesToRemove);
            _cache.ToDictionary(x => x.Key, x => x.Value);
        }

        internal class MemoryCalculator
        {
            private readonly Stream ms;
            private readonly BinaryFormatter formatter;

            public MemoryCalculator()
            {
                ms = new MemoryStream();
                formatter = new BinaryFormatter();
            }

            public long GetSizeInBytes(object obj)
            {
                formatter.Serialize(ms, obj);
                return ms.Length;
            }
        }

        internal class CacheEntry
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
