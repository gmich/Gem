using Gem.Network.Configuration;
using Gem.Network.Handlers;
using System;

namespace Gem.Network.Repositories
{
    public class ClientInfo
    {

        public byte ID { get; set; }

        public Type MessagePoco { get; set; }

        public object EventRaisingclass { get; private set; }

        public IMessageHandler MessageHandler { get; private set; }

    }
}
