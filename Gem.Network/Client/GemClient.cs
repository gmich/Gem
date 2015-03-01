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

        public bool IsRunning { get; set; }

        private readonly IAppender Write;

        private readonly IMessageProcessor messageProcessor;

        private ConnectionDetails connectionDetails;

        private ParallelTaskStarter asyncMessageProcessor;

        #endregion


        #region Constructor

        public GemClient(string serverName, string IPorHost, int port, Action<string> DebugListener = null)
        {
            Guard.That(IPorHost).IsNotNull();
            connectionDetails = new ConnectionDetails { ServerName = serverName, IPorHost = IPorHost, Port = port };

            //TODO: check if the client is already connected
            this.client = GemNetwork.Client;

            this.messageProcessor = new ClientMessageProcessor(client);
            asyncMessageProcessor = new ParallelTaskStarter(TimeSpan.Zero);

            if (DebugListener != null)
            {
                Write = new ActionAppender(DebugListener);
            }
            else
            {
                Write = new Log4NetWrapper("DebugLogger");
            }

            //TODO: register ClientMessageProcesssor's Action<string> Echo    
        }

        #endregion


        #region Start / Close Connection

        public void Disconnect()
        {
            client.Disconnect();
            IsRunning = false;

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
                IsRunning = true;
            }
            catch (Exception ex)
            {
                Write.Error("Unable to connect to {0} . Reason: {1}", connectionDetails.ServerIP, ex.Message);
                IsRunning = false;
            }
        }

        #endregion

    }
}
