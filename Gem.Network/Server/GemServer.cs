using Gem.Network.Async;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;
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

        public GemServer(string serverName, int port, int maxConnections, string password, bool requireAuthentication = false)
        {
            Guard.That(serverName).IsNotNull();

            SetupAuthentication(requireAuthentication);
            config = new ServerConfig { Name = serverName, MaxConnections = maxConnections, Port = port, Password = password };

            //GemNetwork.Server.Dispose();
            server = GemNetwork.Server;

            messageProcessor = new ServerMessageProcessor(server);
            asyncMessageProcessor = new ParallelTaskStarter(TimeSpan.Zero);

            Write = new ActionAppender(GemNetworkDebugger.Echo);
        }

        #endregion


        #region Settings Helpers

        public void SetupAuthentication(bool authenticate)
        {
            if (authenticate)
            {
                GemNetwork.Profile(GemNetwork.ActiveProfile).Server.ForIncomingConnections((server, netconnection, msg) =>
                {
                    if (msg.Password == server.Password)
                    {
                        netconnection.Approve();
                        GemNetworkDebugger.Echo("Approved " + netconnection);
                    }
                    else
                    {
                        GemNetworkDebugger.Echo(String.Format("Declined connection {0}. Reason: Invalid credentials ", netconnection));
                        netconnection.Deny();
                    }
                });
            }
            else
            {
                GemNetwork.Profile(GemNetwork.ActiveProfile).Server.ForIncomingConnections((server, netconnection, msg) =>
                {
                    netconnection.Approve();
                    GemNetworkDebugger.Echo("Approved " + netconnection);
                });
            }
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

    }
}
