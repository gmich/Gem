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

        public IActionDirector OnReceivedServerNotification
        {
            get
            {
                return new ClientActionDirector(profile, MessageType.ServerNotification);
            }
        }

        public IActionDirector OnHandshake
        {
            get
            {
                return new ClientActionDirector(profile, MessageType.Handshake);
            }
        }

        public IActionDirector OnConnecting
        {
            get
            {
                return new ClientActionDirector(profile, MessageType.Connecting);
            }
        }
        public IActionDirector WhenConnected
        {
            get
            {
                return new ClientActionDirector(profile, MessageType.Connected);
            }
        }

        public IActionDirector OnDisconnecting
        {
            get
            {
                return new ClientActionDirector(profile, MessageType.Disconnecting);
            }
        }

        public IActionDirector OnDisconnected
        {
            get
            {
                return new ClientActionDirector(profile, MessageType.Disconnected);
            }
        }

        public IActionDirector HandleWarnings
        {
            get
            {
                return new ClientActionDirector(profile, MessageType.Disconnected);
            }
        }

        public IActionDirector HandleErrors
        {
            get
            {
                return new ClientActionDirector(profile, MessageType.Disconnected);
            }
        }

        public IMessageFlowBuilder CreateNetworkEvent
        {
            get
            {
                return new MessageFlowBuilder(profile, MessageType.Data);
            }
        }

    }

    public class ClientActionDirector : IActionDirector
    {
        private readonly string profile;
        private readonly MessageType messageType;

        public ClientActionDirector(string profile, MessageType messageType)
        {
            this.profile = profile;
            this.messageType = messageType;
        }

        public IMessageFlowBuilder Send(params object[] arguments)
        {
            return new MessageFlowBuilder(profile, messageType);
        }

        public void Do(Action<NetIncomingMessage> action)
        {
            GemNetwork.ClientActionManager[profile, messageType].Add(action);
        }
    }

}



