#region Usings

using Gem.Network.Events;
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

    public sealed class GCache<TKey, TCached> : IDisposable
        where TCached : class
    {

        #region Fields

        private readonly TCached NotFound = default(TCached);

        private readonly long buffer;

        private readonly MemoryCalculator memoryCalculator;

        private Timer stateTimer;

        private long _memoryUsed;

        private long MemoryUsed
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
        

        private ConcurrentDictionary<TKey, CacheEntry> _cache;

        private bool isDisposed;
           
        #endregion

        #region Public Events

        public EventAggregator<TKey, TCached> Events
        {
            get;
            set;
        }

        #endregion

        #region Construct / Dispose

        public GCache(long capacity, IEqualityComparer<TKey> keyEquality , int memoryManagementTime = 300000)
        {
            _cache = new ConcurrentDictionary<TKey, CacheEntry>(keyEquality);
            memoryCalculator = new MemoryCalculator();
            this.isDisposed = false;
            this.buffer = capacity;
            Events = new EventAggregator<TKey, TCached>();

            InitializeMemoryManagementThread(memoryManagementTime);
        }

        private void InitializeMemoryManagementThread(int memoryManagementTime)
        {
            //Process.GetCurrentProcess().PrivateMemorySize64;
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            TimerCallback tcb = this.ManageSize;

            //default invocation every 5 minutes
            stateTimer = new Timer(tcb, autoEvent, memoryManagementTime, memoryManagementTime);
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

        private void ManageSize(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;

            if (MemoryUsed > (buffer * (2 / 3)))
            {
                //if (TryDistinct())
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

        private void FreeResources(long memoryToFree)
        {
            Guard.That(memoryToFree < MemoryUsed,
             "Invalid Operation. The specified memory to free is more than the available memory");

            long currentMemoryUsed = 0;

            while (currentMemoryUsed < memoryToFree )
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

            //TODO: find another way to deallocate, without changing types
            //int entriesToRemove = 0;
            //_cache.ToList().ForEach(x =>
            //{
            //    if (currentMemoryUsed >= memoryToFree) return;
            //    currentMemoryUsed += x.Value.ByteSize;
            //    entriesToRemove++;
            //});
            //_cache.ToList().RemoveRange(0, entriesToRemove);
            //_cache.ToDictionary(x => x.Key, x => x.Value);
        }

        internal class MemoryCalculator
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
