using Gem.Network.Commands;
using Gem.Network.Messages;
using Lidgren.Network;
using System;

namespace Gem.Network.Fluent
{
    public class ClientMessageRouter : IClientMessageRouter
    {
        private readonly string profile;

        public ClientMessageRouter(string profile) 
        {
            this.profile = profile;
        }

        public ActionDirector OnHandshake()
        {
            return new ActionDirector(profile, ClientMessageType.Handshake);
        }

        public ActionDirector OnConnecting()
        {
            return new ActionDirector(profile, ClientMessageType.Connecting);
        }

        public ActionDirector OnConnected()
        {
            return new ActionDirector(profile, ClientMessageType.Connected);
        }

        public ActionDirector OnDisconnecting()
        {
            return new ActionDirector(profile, ClientMessageType.Disconnecting);
        }

        public ActionDirector OnDisconnected()
        {
            return new ActionDirector(profile, ClientMessageType.Disconnected);
        }

        public void HandleWarnings(Action<NetIncomingMessage> action)
        {

        }

        public void HandleErrors(Action<NetIncomingMessage> action)
        {

        }

        public IMessageFlowBuilder CreateNetworkEvent()
        {
            return new MessageFlowBuilder(profile, ClientMessageType.Data);
        }
        
    }

    public class ActionDirector
    {
        private readonly string profile;
        private readonly ClientMessageType messageType;

        public ActionDirector(string profile, ClientMessageType messageType)
        {
            this.profile = profile;
            this.messageType = messageType;
        }

        public IMessageFlowBuilder Send(params object[] arguments)
        {
            return new MessageFlowBuilder(profile, messageType);
        }

        void Do(Action<NetIncomingMessage> action)
        {
            GemNetwork.ClientActionManager[profile, messageType].Add(action);
        }
    }

}



