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

        private readonly INetworkEvent onSay;
        private readonly INetworkEvent onChangeName;

        public Peer(string name)
        {
            this.Name = name;

            onSay = GemNetwork.Profile("Chat")
                  .Client.Send(MessageType.Data)
                  .HandleWith(this, x => new Action<string>(x.Say));

            onChangeName = GemNetwork.Profile("Chat")
                          .Client.Send(MessageType.Data)
                          .HandleWith(this, x => new Action<string>(x.ChangeName));
        }

        public void Say(string message)
        {
            Console.WriteLine("{2} {0} : {1} {2}",Name,message,Environment.NewLine);

            onSay.Send(message);
        }

        public void ChangeName(string newName)
        {
            Console.WriteLine("{2} {0} changed his/her name to: {1} {2}",Name,newName,Environment.NewLine);
            this.Name=newName;

            onChangeName.Send(newName);
        }

    }
}
