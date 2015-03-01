using Gem.Network.Async;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;
using Seterlund.CodeGuard;
using System;

namespace Gem.Network.Client
{
    public class GemClient
    {

        #region Fields

        private readonly IClient client;

        public bool IsConnected { get { return client.IsConnected; } }

        private readonly IAppender Write;

        private readonly IMessageProcessor messageProcessor;

        private ConnectionDetails connectionDetails;

        private ParallelTaskStarter asyncMessageProcessor;

        #endregion


        #region Constructor

        public GemClient(string serverName, string IPorHost, int port)
        {
            Guard.That(IPorHost).IsNotNull();
            connectionDetails = new ConnectionDetails { ServerName = serverName, IPorHost = IPorHost, Port = port };

            //TODO: check if the client is already connected
            this.client = GemNetwork.Client;

            this.messageProcessor = new ClientMessageProcessor(client);
            asyncMessageProcessor = new ParallelTaskStarter(TimeSpan.Zero);
            
            Write = new ActionAppender(GemDebugger.Echo);

            //TODO: register ClientMessageProcesssor's Action<string> Echo    
        }

        #endregion


        #region Start / Close Connection

        public void Disconnect()
        {
            client.Disconnect();
        }

        public void Dispose()
        {
            Disconnect();
        }

        public void RunAsync(Func<ConnectionApprovalMessage> ApprovalMessageDelegate)
        {
            try
            {
                client.Connect(connectionDetails, ApprovalMessageDelegate());
                asyncMessageProcessor.Start(() => messageProcessor.ProcessNetworkMessages());
            }
            catch (Exception ex)
            {
                Write.Error("Unable to connect to {0} . Reason: {1}", connectionDetails.ServerIP, ex.Message);
            }
        }

        #endregion

    }
}
