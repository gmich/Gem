using Gem.Network.Async;
using Gem.Network.Fluent;
using Gem.Network.Handlers;
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

        public GemClient(string profile, string serverName, string IPorHost, int port)
        {
            GemNetwork.ActiveProfile = profile;
            Guard.That(IPorHost).IsNotNull();
            connectionDetails = new ConnectionDetails { ServerName = serverName, IPorHost = IPorHost, Port = port };

            //TODO: check if the client is already connected
            this.client = GemNetwork.Client;

            GemNetwork.ClientMessageFlow[profile, MessageType.Data].Add(new MessageFlowArguments
            {
                MessageHandler = new NotificationHandler(),
                MessagePoco = typeof(ServerNotification),
                ID = 1
            });

            this.messageProcessor = new ClientMessageProcessor(client);
            asyncMessageProcessor = new ParallelTaskStarter(TimeSpan.Zero);
            
            Write = new ActionAppender(GemNetworkDebugger.Echo);

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

        #region Settings

        public static IClientMessageRouter Profile(string profileName)
        {
            return new ClientMessageRouter(profileName);
        }

        public void SendCommand(string command)
        {
            var om = client.CreateMessage();
            var msg = new ServerNotification(GemNetwork.NotificationByte,command);
            MessageSerializer.Encode(msg, ref om);
            client.SendMessage(msg);
        }

        #endregion
    }
}
