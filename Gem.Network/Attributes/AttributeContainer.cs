using Gem.Network.Messages;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Gem.Network.Other
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class IsPrimitiveAttribute : ValidationAttribute
    {

        public override bool IsValid(object type)
        {
            return (!(type.GetType().IsPrimitive || type.GetType() == typeof(String)));
        }


        public override string FormatErrorMessage(string message)
        {
            return String.Format(message);
        }

    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ClientAttribute : Attribute
    {
        public ClientAttribute(params Type[] types)
        {
            this.Types = types;
            this.Configuration = "Default";
        }

        [IsPrimitive]
        public Type[] Types { get; set; }

        public string Configuration { get; set; }
        public string HandleWith { get; set; }

        public NetIncomingMessageType MessageType { get; set; }

        public Action<string> Receiver { get; set; }
    }

    public class Person
    {
        [Client(typeof(string), typeof(int),
                Configuration = "Default",
                MessageType = NetIncomingMessageType.Data,
                HandleWith = "Say")]
        public void Say(string message)
        {
            Console.WriteLine(message);

            //NetworkConfig.Send(message);
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