
using Gem.Network.Messages;
using Lidgren.Network;
using System;

namespace Gem.Network.Events
{
    public class ClientEvent<T> : INetworkEvent
    {

        #region Private Fields

        private event EventHandler<T> Event;

        private readonly IDisposable clientConfig;

        private bool isDisposed;

        private readonly byte Id;

        #endregion


        #region Construct / Dispose

        public ClientEvent(IDisposable clientConfig,byte id)
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


        public void SubscribeEvent(INetworkPeer client)
        {
            Event = (sender, e) => client.SendMessage<T>(e, Id);
        }
                
        public void Send(params object[] networkargs)
        {
            var package = (INetworkPackage)Activator.CreateInstance(typeof(T), networkargs);
            package.Id = Id;
            OnEvent(package);
        }
        
        private void OnEvent(object message)
        {
            EventHandler<T> newPeerEvent = Event;
            if (newPeerEvent != null)
            {
                newPeerEvent(this, (T)message);
            }
        }
             
    }
}
