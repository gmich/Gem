
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

        #endregion


        #region Construct / Dispose

        public ClientEvent(IDisposable clientConfig)
        {
            this.isDisposed = false;
            this.clientConfig = clientConfig;
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


        public void SubscribeEvent(IClient client)
        {
            Event = (sender, e) => client.SendMessage<T>(e);
        }
                
        public void Send(params object[] networkargs)
        {
            var package = Activator.CreateInstance(typeof(T),networkargs);
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
