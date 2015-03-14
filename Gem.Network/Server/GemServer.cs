using Gem.Network.Async;
using Gem.Network.Commands;
using Gem.Network.Fluent;
using Gem.Network.Messages;
using Gem.Network.Providers;
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
        
        private readonly IMessageProcessor messageProcessor;

        private ServerConfig serverConfig;

        private ParallelTaskStarter asyncMessageProcessor;

        private IAppender Write;

        public PackageConfig PackageConfig
        {
            get;
            set;
        }

        public bool IsConnected
        {
            get
            {
                return server.IsConnected;
            }
        }

        #endregion


        #region Constructor

        public GemServer(string profileName, ServerConfig serverConfig,PackageConfig packageConfig)
        {
            Guard.That(serverConfig).IsNotNull();
            Guard.That(packageConfig).IsNotNull();
            
            GemNetwork.ActiveProfile = profileName;

            if (serverConfig.RequireAuthentication)
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

            this.serverConfig = serverConfig;
            this.PackageConfig = packageConfig;

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
            asyncMessageProcessor.Stop();
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
                server.Connect(serverConfig,PackageConfig);
                asyncMessageProcessor.Start(() => messageProcessor.ProcessNetworkMessages());
            }
            catch (Exception ex)
            {
                Write.Error("Unable to start the server. Reason: {0}", ex.Message);
            }
        }

        #endregion


        #region Settings

        private static ServerConfigurationManager serverConfigurationManager;
        internal static ServerConfigurationManager ServerConfiguration
        {
            get
            {
                return serverConfigurationManager
                      = serverConfigurationManager ?? new ServerConfigurationManager();
            }
        }

        public static IServerMessageRouter Profile(string profileName)
        {
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
