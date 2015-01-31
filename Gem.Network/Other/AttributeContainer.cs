using Gem.Network.Messages;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Other
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ClientAttribute : Attribute
    {
        public ClientAttribute()
        {
            this.Configuration = "Default";
        }


        public string Configuration { get; set; }

        public NetIncomingMessageType MessageType { get; set; }

        public Action<string> Receiver { get; set; }
    }

    public class Person
    {
        [Client(MessageType = NetIncomingMessageType.ConnectionApproval,
            Configuration = "Default")]
        public void Say(string message)
        {

        }


    }

    public class Message : IServerMessage
    {
        public string Text { get; set; }

        public IncomingMessageTypes MessageType
        {
            get { throw new NotImplementedException(); }
        }

        public void Decode(NetIncomingMessage im)
        {
            throw new NotImplementedException();
        }

        public void Encode(NetOutgoingMessage om)
        {
            throw new NotImplementedException();
        }
    }


}
