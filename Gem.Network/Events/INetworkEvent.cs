using System;

namespace Gem.Network.Events
{
    public interface INetworkEvent : IDisposable
    {
        void SubscribeEvent(INetworkPeer client);

        void Send(params object[] networkargs);
    }
}
