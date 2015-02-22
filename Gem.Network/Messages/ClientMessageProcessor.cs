using Gem.Network.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Messages
{
    class ClientMessageProcessor :IMessageProcessor
    {
        private readonly ClientConfig config;

        public ClientMessageProcessor(ClientConfig config)
        {
            this.config = config;
        }

        public void ProcessMessage(Lidgren.Network.NetIncomingMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
