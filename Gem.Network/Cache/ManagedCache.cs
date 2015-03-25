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
    /// A cache that uses a callback to free resources
    /// </summary>
    /// <typeparam name="TKey">The lookup's key</typeparam>
    /// <typeparam name="TCached">The stored object</typeparam>
    public sealed class ManagedCache<TKey, TCached> : GCache<TKey, TCached>
        where TCached : class
    {

        #region Fields

        private readonly Timer stateTimer;

        #endregion

        #region Construct / Dispose

        /// <summary>
        /// Instantiate a managed cache
        /// </summary>
        /// <param name="capacity">The buffer's capacity</param>
        /// <param name="keyEquality">How the lookup is performed</param>
        /// <param name="memoryManagementTime">How long until the memory management is performed</param>
        public ManagedCache(long capacity, IEqualityComparer<TKey> keyEquality, int memoryManagementTime = 300000)
            : base(capacity, keyEquality)
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            TimerCallback tcb = this.ManageSizeAsync;

            //default invocation every 5 minutes
            stateTimer = new Timer(tcb, autoEvent, memoryManagementTime, memoryManagementTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                _cache = null;
                isDisposed = true;
                stateTimer.Dispose();
            }
        }

        #endregion
        
        #region Memory Management

        protected override void ManageSize() { return; }

        private void ManageSizeAsync(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;

            if (MemoryUsed > ((buffer * 2) / 3))
            {
                FreeResources(MemoryUsed / 4);
            }
        }

        #endregion

    }
}

