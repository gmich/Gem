using System;

namespace Gem.Network.Server
{
    public interface IProtocolServerEvent
    {
        void Dispose();
        void Send(Lidgren.Network.NetConnection sender, object netpackage);
        void SubscribeEvent(Gem.Network.INetworkPeer server);
    }
}
