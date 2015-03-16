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

        public static Dictionary<NetConnection, string> ConnectedPeers = new Dictionary<NetConnection, string>();

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
                if (msg.Type == NotificationType.Command)
                {
                    GemServer.ExecuteCommand(connection, msg.Message);
                }
                if (msg.Type == NotificationType.Message)
                {
                    if (msg.Message.StartsWith("newname"))
                    {
                        var arguments = msg.Message.Split(' ');
                        var key = ConnectedPeers.Where(x => x.Key.RemoteEndpoint.Equals(connection.RemoteEndpoint)).Select(x => x.Key).First();
                        if (key != null)
                        {
                            ConnectedPeers[key] = arguments[1];
                            Console.WriteLine("Updated referenced name " + arguments[1]);
                            return;
                        }
                        Console.WriteLine("Unable to update name");
                    }
                }
            });

            GemServer.Profile(ActiveProfile).OnClientDisconnect((server, connection, msg) =>
            {
                server.NotifyAll("[Disconnected client]" + msg);
            });

            GemServer.RegisterCommand("whoison", "Show the connected clients by name", false,
                (server, sender, command, arguments) =>
                {
                    server.NotifyOnly(String.Join(Environment.NewLine, ConnectedPeers.Select(x => x.Value)),sender);
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
                                (server, connection, message) =>
                                ConnectedPeers.Add(connection, message.Sender),
                                append: true);

           GemServer.Profile(ActiveProfile).OnClientDisconnect(
                                (server, connection, message) =>
                                ConnectedPeers.Remove(connection),
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
