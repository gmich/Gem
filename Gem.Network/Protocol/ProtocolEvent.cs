
using System;
using System.Linq;

namespace Gem.Network.Events
{
    using Extensions;

    /// <summary>
    /// Raises events by serializing and sending packages.
    /// Works for classes that are annotated with the NetworkPackageAttribute
    /// via socket
    /// </summary>
    public class ProtocolEvent<T> : INetworkEvent
    {

        #region Private Fields

        private event EventHandler<T> Event;

        /// <summary>
        /// The MessageFlowArgument's disposable.
        /// By disposing the MessageFlowArguments are removed from the cache
        /// </summary>
        private readonly IDisposable clientConfig;

        private bool isDisposed;

        private readonly byte Id;

        #endregion


        #region Construct / Dispose

        public ProtocolEvent(IDisposable clientConfig, byte id)
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

        /// <summary>
        /// Sends objects that are annotated with NetworkPackageAttribute
        /// </summary>
        /// <param name="networkargs">The object to send</param>
        public void Send(params object[] networkargs)
        {
            var package = (INetworkPackage)Activator.CreateInstance(
                           typeof(T),
                           networkargs.First().ReadAllProperties());
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
