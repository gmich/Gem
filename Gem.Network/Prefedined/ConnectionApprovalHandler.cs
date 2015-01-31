using Gem.Network.Messages;
using Gem.Network.Utilities;
using Lidgren.Network;
using System;

namespace Gem.Network.Prefedined
{
    public class ConnectionApprovalHandler : IMessageConfiguration<NetIncomingMessage, IServerMessage>
    {
        public IncomingMessageTypes ServerIncomingMesssageType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Func<NetIncomingMessage, IServerMessage> Converter
        {
            set { throw new NotImplementedException(); }
        }

        public IDisposable AddHandler(Action<IServerMessage> messageHandler)
        {
            throw new NotImplementedException();
        }

        public void Handle(NetIncomingMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
