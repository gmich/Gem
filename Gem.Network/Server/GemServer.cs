using Gem.Network.Async;
using Gem.Network.Commands;
using Gem.Network.Fluent;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;
using Lidgren.Network;
using Seterlund.CodeGuard;
using System;

namespace Gem.Network.Server
{
    public class GemServer
    {

        #region Fields

        private readonly IServer server;

        public bool IsConnected { get { return server.IsConnected; } }

        private readonly IMessageProcessor messageProcessor;

        private ServerConfig config;

        private ParallelTaskStarter asyncMessageProcessor;

        private IAppender Write;

        #endregion


        #region Constructor

        public GemServer(string profileName, string serverName, int port, int maxConnections, string password, bool requireAuthentication = false)
        {
            Guard.That(profileName).IsNotNull();
            Guard.That(serverName).IsNotNull();
            
            GemNetwork.ActiveProfile = profileName;

            if (requireAuthentication)
            {
                RequireAuthentication();
            }
            else
            {
                Profile(GemNetwork.ActiveProfile).OnIncomingConnection((srvr, netconnection, msg) =>
                {
                    netconnection.Approve();
                    GemNetworkDebugger.Echo(String.Format("Approved {0} {3} Sender: {1}{3} Message: {2}"
                                            , netconnection, msg.Sender, msg.Message, Environment.NewLine));
                });
            }

            config = new ServerConfig { Name = serverName, MaxConnections = maxConnections, Port = port, Password = password };

            //GemNetwork.Server.Dispose();
            server = GemNetwork.Server;

            messageProcessor = new ServerMessageProcessor(server);
            asyncMessageProcessor = new ParallelTaskStarter(TimeSpan.Zero);

            Write = new ActionAppender(GemNetworkDebugger.Echo);
        }

        #endregion


        #region Settings Helpers

        private void RequireAuthentication()
        {
            Profile(GemNetwork.ActiveProfile).OnIncomingConnection((svr, netconnection, msg) =>
            {
                if (msg.Password == server.Password)
                {
                    netconnection.Approve();
                    GemNetworkDebugger.Echo(String.Format("Approved {0} {3} Sender: {1}{3} Message: {2}"
                                            ,netconnection,msg.Sender,msg.Message,Environment.NewLine));
                }
                else
                {
                    GemNetworkDebugger.Echo(String.Format("Declined connection {0}. Reason: Invalid credentials {4} Sender: {1}{4} Message: {2}{4} Password: {3}"
                                           ,netconnection,msg.Sender,msg.Message,msg.Password,Environment.NewLine));
                    netconnection.Deny();
                }
            });
        }
    

        #endregion


        #region Start / Close Connection

        public void Disconnect()
        {
            server.Disconnect();
        }

        public void Dispose()
        {
            Disconnect();
        }

        public void RunAsync()
        {
            try
            {
                server.Connect(config);
                asyncMessageProcessor.Start(() => messageProcessor.ProcessNetworkMessages());
            }
            catch (Exception ex)
            {
                Write.Error("Unable to start the server. Reason: {0}", ex.Message);
            }
        }

        #endregion


        #region Static Settings

        public static IServerMessageRouter Profile(string profileName)
        {
            GemNetwork.dynamicMessagesCreated++;

            return new ServerMessageRouter(profileName);

        }

        public static void RegisterCommand(string command,string description,bool requireAuthorization,CommandExecute callback)
        {
            GemNetwork.Commander.RegisterCommand(command, requireAuthorization, description, callback);
        }

        public static void SetConsolePassword(string password)
        {
            GemNetwork.Commander.SetPassword(password);
        }

        public static void ExecuteCommand(string command)
        {
            GemNetwork.Commander.ExecuteCommand(null,command);
        }

        #endregion

    }
}
