using Gem.Network.Server;
using Gem.Network.Utilities.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Gem.Network.Chat.Server
{
    class ChatServer
    {
        static void Main(string[] args)
        {
            GemNetwork.ActiveProfile = "GemChat";
            GemNetworkDebugger.Echo = Console.WriteLine;

            GemServer gemServer = new GemServer("GemChat", 14242, 10,"123456");

            gemServer.RunAsync();

            while (Console.ReadLine() != "quit") ;
        }
    }
}
