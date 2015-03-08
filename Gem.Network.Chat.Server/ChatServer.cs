using Gem.Network.Server;
using Gem.Network.Utilities.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Gem.Network.Commands;

namespace Gem.Network.Chat.Server
{
    class ChatServer
    {
        static void Main(string[] args)
        {
            var ActiveProfile = "GemChat";
            GemNetworkDebugger.Echo = Console.WriteLine;

            GemServer gemServer = new GemServer(ActiveProfile, "GemChat", 14242, 10, "gem", requireAuthentication: true);
            //GemServer.SetConsolePassword("123");
            gemServer.RunAsync();

            while (true)
            {
                GemServer.ExecuteCommand(Console.ReadLine());
            }
        }
    }
}
