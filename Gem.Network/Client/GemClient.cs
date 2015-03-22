using Gem.Network.Async;
using Gem.Network.Fluent;
using Gem.Network.Handlers;
using Gem.Network.Managers;
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

        private ConnectionConfig connectionDetails;

        private ParallelTaskStarter asyncMessageProcessor;

        public PackageConfig PackageConfig
        {
            get;
            set;
        }

        #endregion
        
        #region Constructor

        public GemClient(string profile, ConnectionConfig connectionConfig, PackageConfig packageConfig )
        {            
            Guard.That(connectionConfig).IsNotNull();
            Guard.That(packageConfig).IsNotNull();

            PackageConfig=packageConfig;
            connectionDetails = connectionConfig;
            GemNetwork.ActiveProfile = profile;

            //TODO: check if the client is already connected
            this.client = GemNetwork.Client;
            this.client.PackageConfig = PackageConfig;

            RegisterServerNotificationPackages(profile);

            messageProcessor = new ClientMessageProcessor(client);
            asyncMessageProcessor = new ParallelTaskStarter(TimeSpan.Zero);
            
            Write = new ActionAppender(GemNetworkDebugger.Echo);
        }

        #endregion

        #region Private Helpers

        private void RegisterServerNotificationPackages(string profile)
        {
            if (!GemClient.MessageFlow[profile, MessageType.Data].HasKey(GemNetwork.NotificationByte))
            {
                GemClient.MessageFlow[profile, MessageType.Data].Add(new MessageFlowArguments
                {
                    MessageHandler = new NotificationHandler(),
                    MessagePoco = typeof(Notification),
                    ID = GemNetwork.NotificationByte
                });
            }
        }

        #endregion

        #region Start / Close Connection

        public void Disconnect()
        {
            asyncMessageProcessor.Stop();
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
                client.Connect(connectionDetails, PackageConfig,ApprovalMessageDelegate());
                asyncMessageProcessor.Start(() => messageProcessor.ProcessNetworkMessages());
            }
            catch (Exception ex)
            {
                Write.Error("Unable to connect to {0} . Reason: {1}", connectionDetails.ServerIP, ex.Message);
            }
        }

        #endregion

        #region Settings

        private static ClientMessageFlowManager clientMessageFlowManager;
        internal static ClientMessageFlowManager MessageFlow
        {
            get 
            { 
                return  clientMessageFlowManager 
                      = clientMessageFlowManager?? new ClientMessageFlowManager();
            }
        }

        private static ClientNetworkActionManager clientActionManager;
        internal static ClientNetworkActionManager ActionManager
        {
            get
            {
                return clientActionManager
                     = clientActionManager ?? new ClientNetworkActionManager();
            }
        }

        public static IClientMessageRouter Profile(string profileName)
        {
            return new ClientMessageRouter(profileName);
        }

        public static void SendCommand(string command)
        {
            var om = GemNetwork.Client.CreateMessage();
            var msg = new Notification(command, NotificationType.Command);
            MessageSerializer.Encode(msg, ref om);
            GemNetwork.Client.SendMessage(msg);
        }

        public static void NotifyServer(string message)
        {
            var om = GemNetwork.Client.CreateMessage();
            var msg = new Notification(message, NotificationType.Message);
            MessageSerializer.Encode(msg, ref om);
            GemNetwork.Client.SendMessage(msg);
        }

        #endregion
    }
}
