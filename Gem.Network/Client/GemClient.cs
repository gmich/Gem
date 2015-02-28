using Gem.Network.Async;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;
using System;

namespace Gem.Network.Client
{
    public class GemClient
    {

        #region Fields

        private readonly IClient client;

        public bool IsRunning { get; set; }

        public Action<string> Echo;

        private readonly IAppender Write;

        private readonly IMessageProcessor messageProcessor;

        private readonly ConnectionDetails connectionDetails;

        private ParallelTaskStarter asyncMessageProcessor;
        
        #endregion


        #region Constructor

        public GemClient(ConnectionDetails connectionDetails)
        {
            this.connectionDetails = connectionDetails;
            this.client = new Peer();
            this.messageProcessor = new ClientMessageProcessor(client);
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
          
        public void RunAsync()
        {
            try
            {
                client.Connect(connectionDetails);
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
