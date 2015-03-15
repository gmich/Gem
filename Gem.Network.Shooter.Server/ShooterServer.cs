using Gem.Network.Messages;
using Gem.Network.Server;
using Gem.Network.Utilities.Loggers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Shooter.Server
{
    class ChatServer
    {
        const string ActiveProfile = "Shooter";

        public static Dictionary<NetConnection, string> ConnectedPeers = new Dictionary<NetConnection, string>();

        static void Main(string[] args)
        {
            GemNetworkDebugger.Echo = Console.WriteLine;

            GemServer gemServer = new GemServer(ActiveProfile, new ServerConfig
            {
                Name = "Shooter",
                Port = 14242,
                EnableUPnP = false,
                MaxConnections = 10,
                Password = "gem",
                ConnectionTimeout = 5.0f,
                RequireAuthentication = false
            },
            PackageConfig.UDPSequenced);

            GemServer.Profile(ActiveProfile).HandleNotifications((server, connection, msg) =>
            {
                if (msg.Type == NotificationType.Command)
                {
                    GemServer.ExecuteCommand(connection, msg.Message);
                }
                if (msg.Type == NotificationType.Message)
                {  }
            });

            GemServer.Profile(ActiveProfile).OnClientDisconnect((server, connection, msg) =>
            {
                server.NotifyAll("[Disconnected client]" + msg);
            });

            GemServer.RegisterCommand("whoison", "Show the connected clients by name", false,
                (server, sender, command, arguments) =>
                {
                    server.NotifyOnly(String.Join(Environment.NewLine, ConnectedPeers.Select(x => x.Value)), sender);
                });

            GemServer.RegisterCommand("kickbyname", "Kick client by his name", true,
            (server, sender, command, arguments) =>
            {
                if (arguments.Count == 1)
                {
                    if (ConnectedPeers.ContainsValue(arguments[0]))
                    {
                        server.Kick(ConnectedPeers.Where(x => x.Value == arguments[0]).Select(x => x.Key).First().RemoteEndpoint,
                                    " kicked " + arguments[0]);
                        server.NotifyOnly("Kicked " + arguments[0], sender);
                    }
                    else
                    {
                        server.NotifyOnly("Client not found", sender);
                    }
                }
                else
                {
                    server.NotifyOnly("Insufficient number of arguments", sender);

                }
            });

            GemServer.Profile(ActiveProfile).OnIncomingConnection(
                                 (server, sender, message) =>
                                 {
                                     foreach (var peer in ConnectedPeers)
                                     {
                                         server.NotifyOnly("newplayer " + peer.Value, sender, NotificationType.Command);
                                     }
                                     server.NotifyAllExcept("newplayer " + message.Sender,sender, NotificationType.Command);
                                     ConnectedPeers.Add(sender, message.Sender);
                                 },
                                 append: true);

            GemServer.Profile(ActiveProfile).OnClientDisconnect(
                                 (server, connection, message) =>
                                 {
                                     server.NotifyAll("removeplayer " + ConnectedPeers[connection], NotificationType.Command);
                                     ConnectedPeers.Remove(connection);
                                 },
                                 append: true);

            GemServer.SetConsolePassword("gem");

            gemServer.RunAsync();

            while (true)
            {
                GemServer.ExecuteCommand(Console.ReadLine());
            }
        }
    }
}
