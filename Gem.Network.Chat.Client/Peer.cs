using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gem.Network.Messages;
using Gem.Network.Events;

namespace Gem.Network.Chat.Client
{
    public class Peer
    {
        public string Name { get; set; }

        private readonly INetworkEvent onEvent;

        public Peer(string name)
        {
            this.Name = name;

            onEvent = GemNetwork.Profile("GemChat")
                  .Client.Send(MessageType.Data)
                  .HandleWith(this, x => new Action<string>(x.Print));

            onEvent.Send(name + " has joined");
        }

        public void Say(string message)
        {
            string formattedMessage = String.Format(" >> {0} : {1}", Name, message);
            Console.WriteLine("{0} : {1}", Name, message);

            onEvent.Send(formattedMessage);
        }

        public void ChangeName(string newName)
        {
            string formattedMessage = String.Format(" >> {0} changed his/her name to {1}", Name, newName);

            Console.WriteLine("you have changed your name to {0}", newName);

            this.Name = newName;

            onEvent.Send(formattedMessage);
        }

        public void Print(string message)
        {
            Console.WriteLine(message);
        }

        public void SayGoodBye()
        {
            onEvent.Send(" >> " + Name + " has left ");
        }

    }
}
