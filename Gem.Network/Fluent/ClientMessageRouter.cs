using Gem.Network.Client;
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

        public void OnReceivedServerNotification(Action<Notification> action, bool append = false)
        {
            if (append)
            {
                GemClient.ActionManager[profile].OnReceivedNotification += action;
            }
            else
            {
                GemClient.ActionManager[profile].OnReceivedNotification = action;
            }
        }

        public void OnConnecting(Action<IClient, NetIncomingMessage> action, bool append = false)
        {
            Do(action, MessageType.Connecting, append);

        }
        public void WhenConnected(Action<IClient, NetIncomingMessage> action, bool append = false)
        {
            Do(action, MessageType.Connected, append);
        }

        public void OnDisconnecting(Action<IClient, NetIncomingMessage> action, bool append = false)
        {
            Do(action, MessageType.Disconnecting, append);
        }

        public void OnDisconnected(Action<IClient, NetIncomingMessage> action, bool append = false)
        {
            Do(action, MessageType.Disconnected, append);
        }

        public void HandleWarnings(Action<IClient, NetIncomingMessage> action, bool append = false)
        {
            throw new NotImplementedException();
        }

        public void HandleErrors(Action<IClient, NetIncomingMessage> action, bool append = false)
        {
            throw new NotImplementedException();
        }

        public IMessageFlowBuilder CreateNetworkEvent
        {
            get
            {
                return new MessageFlowBuilder(profile, MessageType.Data);
            }
        }

        public IMessageFlowBuilder CreateNetworkEventWithRemoteTime
        {
            get
            {
                return new MessageFlowRemoteTimeBuilder(profile, MessageType.Data);
            }
        }

        private void Do(Action<IClient, NetIncomingMessage> action, MessageType messageType, bool append)
        {
            if (append)
            {
                GemClient.ActionManager[profile, messageType].Action += action;
            }
            else
            {
                GemClient.ActionManager[profile, messageType].Action = action;
            }
        }
    }
}



