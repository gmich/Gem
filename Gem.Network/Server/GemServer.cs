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

        public GemServer(string serverName, int port,int maxConnections, string password = null)
        {
            Guard.That(serverName).IsNotNull();
            config = new ServerConfig {  Name = serverName, MaxConnections = maxConnections , Port = port ,Password = null};
            
            //GemNetwork.Server.Dispose();
            server = GemNetwork.Server;

            messageProcessor = new ServerMessageProcessor(server);
            asyncMessageProcessor = new ParallelTaskStarter(TimeSpan.Zero);

            Write = new ActionAppender(GemNetworkDebugger.Echo);            
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
