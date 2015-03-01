using Gem.Network.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Chat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            GemNetwork.ActiveProfile = "GemChat";

            GemServer server = new GemServer("GemChat", 14242, 10);

            server.RunAsync();

            while (Console.ReadLine() != "quit") ;
        }
    }
}
