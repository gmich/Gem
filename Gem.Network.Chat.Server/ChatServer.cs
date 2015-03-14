using Gem.Network.Server;
using Gem.Network.Utilities.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Gem.Network.Commands;
using Gem.Network.Messages;

namespace Gem.Network.Chat.Server
{
    class ChatServer
    {
        const string ActiveProfile = "GemChat";

        static void Main(string[] args)
        {
            GemNetworkDebugger.Echo = Console.WriteLine;

            GemServer gemServer = new GemServer(ActiveProfile, new ServerConfig
            {
                Name = "GemChat",
                Port = 14242,
                EnableUPnP = false,
                MaxConnections = 10,
                Password = "gem",
                ConnectionTimeout = 5.0f,
                RequireAuthentication = true
            },
                PackageConfig.TCP);

            GemServer.Profile(ActiveProfile).HandleNotifications((server, connection, msg) =>
            {
                if (msg.Type == "command")
                {
                    GemServer.ExecuteCommand(msg.Message);
                }
            });

            //GemServer.SetConsolePassword("123");
            gemServer.RunAsync();

            while (true)
            {
                GemServer.ExecuteCommand(Console.ReadLine());
            }
        }
    }
}
