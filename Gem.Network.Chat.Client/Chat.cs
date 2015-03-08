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

        #region Fields

        private static Peer peer;
        private static GemClient client;

        #endregion

        #region Private Helpers

        private static void PrintIntroMessage()
        {
            Console.WriteLine(String.Format(
            @" 
             Commands {0}
-------------------------------------{0}
-gem <command>      |  Gem console        
-setname <newname>  |  Change nickname  
-quit               |  Quit  {0}{0}", Environment.NewLine));

        }

        private static void ClientSetup()
        {
           // GemNetwork.ActiveProfile = "GemChat";
            GemNetworkDebugger.Echo = Console.WriteLine;

            client = new GemClient("GemChat","GemChat", "127.0.0.1", 14242);
        }

        private static void ProcessInput()
        {
            string input = string.Empty;
            while (input != "-quit")
            {
                input = Console.ReadLine();
              //  Console.SetCursorPosition(0, Console.CursorTop - 1);
                if (input.Length >= 1)
                {
                    if (input.StartsWith("-gem "))
                    {
                        var cmd = input.Substring(5);
                        if (cmd.Length > 0)
                        {
                            client.SendCommand(cmd);
                        }
                    }
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
            }
        }

        #endregion

        static void Main(string[] args)
        {
            ClientSetup();

            //Pick a name
            Console.WriteLine("Your nickname : ");
            string name = Console.ReadLine();

            Console.WriteLine("Password : ");
            string pwd = Console.ReadLine();

            PrintIntroMessage();

            client.RunAsync(() => new ConnectionApprovalMessage { Message = "Incoming client", Sender = name, Password = pwd });

            //Initialize a chat peer
            peer = new Peer(name);

            ProcessInput();

            peer.SayGoodBye();

            client.Dispose();

            Console.ReadLine();
        }

    }
}
