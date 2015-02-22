using System;

namespace Gem.Network.Events
{
    public interface INetworkEvent : IDisposable
    {
        void SubscribeEvent(IClient client);

        void Send(params object[] networkargs);
    }
}
