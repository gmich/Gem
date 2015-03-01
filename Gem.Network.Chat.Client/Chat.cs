using Gem.Network.Client;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Chat.Client
{
    class Chat
    {
        static void Main(string[] args)
        {
            GemNetwork.ActiveProfile = "GemChat";
            GemDebugger.Echo = Console.WriteLine;

            var client = new GemClient("GemChat", "127.0.0.1", 14242);

            Console.WriteLine("Your nickname : ");
            var peer = new Peer(Console.ReadLine());

            Console.WriteLine(String.Format(
            @" 
             Commands {0}
-------------------------------------{0}
-setname <newname>  |  Change nickname  
-quit               |  Quit  {0}{0}", Environment.NewLine));

            client.RunAsync(() => new ConnectionApprovalMessage { Sender = "Dummy" , Password = "123" } );
            
            string input = string.Empty;
            while(input!="-quit")
            {
                input = Console.ReadLine();
                Console.SetCursorPosition(input.Length,Console.CursorTop-2);
                    if (input[0] == '-')
                    {
                        var processed = input.Split(' ');
                        if (processed.Length >= 2)
                        {
                            if (processed[0] == "-setname")
                            {
                                peer.ChangeName(processed[1]);
                            }
                        }
                    }
                    else
                    {
                        peer.Say(input);
                    }                
            }

            client.Dispose();

            Console.ReadLine();
        }

    }
}
