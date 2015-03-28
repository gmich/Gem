using Gem.Network.Messages;
using Lidgren.Network;
using System.Linq;
using System;

namespace Gem.Network.Events
{
    /// <summary>
    /// Invokes client-side events that serialize and send packages to the connected endpoint server
    /// The packages include the time the current client is active
    /// </summary>
    /// <typeparam name="TMessageType">The type of the outgoing message</typeparam>
    public class RemoteTimeEvent<TMessageType> : INetworkEvent
    {

        #region Private Fields

        /// <summary>
        /// The handler
        /// </summary>
        private event EventHandler<TMessageType> Event;

        /// <summary>
        /// The MessageFlowArguments disposable
        /// </summary>
        private readonly IDisposable clientConfig;

        /// <summary>
        /// True if the ClientEvent and clientConfig are disposed
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// The events unique identifier
        /// </summary>
        private readonly byte Id;

        #endregion
        
        #region Construct / Dispose

        /// <summary>
        /// Sets up a new instance of RemoteTimeEvent<>
        /// </summary>
        /// <param name="clientConfig">The MessageFlowArguments disposable</param>
        /// <param name="id">The events unique identifier</param>
        public RemoteTimeEvent(IDisposable clientConfig, byte id)
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


        /// <summary>
        /// Subscribes the send event to the connected client.
        /// When Send(params object[] networkargs) is invoked, an event is raised,
        /// sending a message from the connected client to the server 
        /// </summary>
        /// <param name="client">The client that sends the message</param>
        public void SubscribeEvent(INetworkPeer client)
        {
            Event = (sender, e) => client.SendMessage<TMessageType>(e, Id);
        }

        /// <summary>
        /// Sends an event via the client by raising an event. The package also includes the active client time
        /// </summary>
        /// <param name="networkargs">The arguments that are sent. The package's class is initialized via Activator,
        /// so the arguments are the parameters of type T constructor</param>
        public void Send(params object[] networkargs)
        {
            var args = networkargs.Select(x => x).ToList();
            args.Add(NetTime.Now);
            SendWithTime(args.ToArray());
        }

        /// <summary>
        /// Helper class that class OnEvent and raised the event
        /// </summary>
        /// <param name="networkargs">The parameters of type TMessageType with the client time</param>
        private void SendWithTime(params object[] networkargs)
        {
            var package = (INetworkPackage)Activator.CreateInstance(typeof(TMessageType), networkargs);
            package.Id = Id;
            OnEvent(package);
        }

        /// <summary>
        /// Raises the event
        /// </summary>
        /// <param name="message">The message that's being sent</param>
        private void OnEvent(object message)
        {
            EventHandler<TMessageType> newPeerEvent = Event;
            if (newPeerEvent != null)
            {
                newPeerEvent(this, (TMessageType)message);
            }
        }

    }
}
