
using Gem.Network.Messages;
using Lidgren.Network;
using System;
using System.Linq;

namespace Gem.Network.Events
{
    using Extensions;
    using Gem.Network.Server;

    public class ProtocolServerEvent<T> : Gem.Network.Server.IProtocolServerEvent
    {

        #region Private Fields

        private event EventHandler<T> Event;

        private readonly IDisposable clientConfig;

        private bool isDisposed;

        private readonly byte Id;

        #endregion


        #region Construct / Dispose

        public ProtocolServerEvent(IDisposable clientConfig, byte id)
        {
            this.isDisposed = false;
            this.clientConfig = clientConfig;
            this.Id = id;
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
                clientConfig.Dispose();
            }
        }

        #endregion


        public void SubscribeEvent(INetworkPeer server)
        {
            Event = (sender, e) => (server as IServer).SendMessage<T>(sender as NetConnection, e, Id);
        }

        public void Send(NetConnection sender,object netpackage)
        {
            var package = (INetworkPackage)Activator.CreateInstance(typeof(T), netpackage.ReadAllProperties());
            package.Id = Id;
            OnEvent(sender,package);
        }
        
        private void OnEvent(NetConnection sender, object message)
        {
            EventHandler<T> newPeerEvent = Event;
            if (newPeerEvent != null)
            {
                newPeerEvent(sender, (T)message);
            }
        }
             
    }
}
