
using Lidgren.Network;
using Gem.Network.Server;
using System;

namespace Gem.Network.Events
{
    using Extensions;

    /// <summary>
    /// Invokes server-side events that serialize and send packages to the connected peers
    /// </summary>
    /// <typeparam name="TNetworkPackage">The type of the outgoing message</typeparam>
    public class ProtocolServerEvent<TNetworkPackage> : IProtocolServerEvent
    {

        #region Private Fields

        /// <summary>
        /// The event handler
        /// </summary>
        private event EventHandler<TNetworkPackage> Event;

        /// <summary>
        /// The <see cref=">MessageArguments"/> disposable
        /// </summary>
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
            Event = (sender, e) => (server as IServer)
                                   .SendMessage<TNetworkPackage>(sender as NetConnection, e, Id);
        }

        /// <summary>
        /// Raise the event that sends a message and excludes a connection
        /// </summary>
        /// <param name="excluded">The excluded connection</param>
        /// <param name="netpackage">The message to serialize and send</param>
        public void Send(NetConnection sender,object netpackage)
        {
            var package = (INetworkPackage)Activator.CreateInstance(typeof(TNetworkPackage), netpackage.ReadAllProperties());
            package.Id = Id;
            OnEvent(sender,package);
        }
        
        private void OnEvent(NetConnection sender, object message)
        {
            EventHandler<TNetworkPackage> newPeerEvent = Event;
            if (newPeerEvent != null)
            {
                newPeerEvent(sender, (TNetworkPackage)message);
            }
        }
             
    }
}
