using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gem.Infrastructure.Cache.Events
{
    /// <summary>
    /// Provides event for GCache
    /// </summary>
    /// <typeparam name="Targs"></typeparam>
    public interface IEventProvider<Targs>
        where Targs : EventArgs
    {
        /// <summary>
        /// Raised when an item is added to the cache
        /// </summary>
        event EventHandler<Targs> OnAddEvent;

        /// <summary>
        /// Raised when theres a change in the buffer 
        /// e.g. deallocation
        /// </summary>
        event EventHandler<Targs> OnUsedMemoryEvent;
    }
    
}
