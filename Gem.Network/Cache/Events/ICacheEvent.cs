using System;

namespace Gem.Network.Events
{

    public interface ICacheEvent<Tsender, Targs> : IEventProvider<Targs>
        where Targs : EventArgs
    {

        void RaiseAddEvent(Tsender sender, Targs args);

        void RaiseUsedMemoryChangedEvent(Tsender sender, Targs args);

    }
}
