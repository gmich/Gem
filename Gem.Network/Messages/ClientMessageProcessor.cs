using Gem.Network.Configuration;
using Gem.Network.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Messages
{
    class ClientMessageProcessor :IMessageProcessor
    {
        private readonly ClientConfigurationContainer config;

        public ClientMessageProcessor(ClientConfigurationContainer config)
        {
            this.config = config;
        }

        public void ProcessMessage(Lidgren.Network.NetIncomingMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
