using Gem.Network.Async;
using Gem.Network.Fluent;
using Gem.Network.Handlers;
using Gem.Network.Managers;
using Gem.Network.Messages;
using Gem.Network.Server;
using Gem.Network.Utilities.Loggers;
using Seterlund.CodeGuard;
using System;

namespace Gem.Network.Client
{
    /// <summary>
    /// The class that handles the client side connection , message processing and configuration
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class GemClient
    {

        #region Fields

        /// <summary>
        /// The client
        /// </summary>
        private readonly IClient client;

        /// <summary>
        /// Shows if the client has an active connection to a server
        /// </summary>
        public bool IsConnected { get { return client.IsConnected; } }

        private readonly IMessageProcessor messageProcessor;

        private ConnectionConfig connectionDetails;

        /// <summary>
        /// This is used to process incoming messages asynchronously and not in the program's main thread
        /// </summary>
        private ParallelTaskStarter asyncMessageProcessor;

        /// <summary>
        /// The outgoing messages configuration
        /// </summary>
        public PackageConfig PackageConfig
        {
            get;
            set;
        }

        #endregion
        
        #region Constructor

        /// <summary>
        /// Initializes a new instance of GemClient.
        /// </summary>
        /// <param name="profile">The profile the client's configuration is set</param>
        /// <param name="connectionConfig">The configuration for the connection</param>
        /// <param name="packageConfig">The configuration for the outgoing messages</param>
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
            
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Registers the notification messages.
        /// This is preserved to be the GemNetwork.NotificationByte and is an essential part of GemNetwork
        /// </summary>
        /// <param name="profile">The active profile</param>
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

        /// <summary>
        /// Tries to connect and starts receiving / sending messages
        /// </summary>
        /// <param name="ApprovalMessageDelegate">A delegate that returns a connection approval message thats handled by the server</param>
        public void RunAsync(Func<ConnectionApprovalMessage> ApprovalMessageDelegate)
        {
            try
            {
                client.Connect(connectionDetails, PackageConfig,ApprovalMessageDelegate());
                asyncMessageProcessor.Start(() => messageProcessor.ProcessNetworkMessages());
            }
            catch (Exception ex)
            {
                GemNetworkDebugger.Append.Error("Unable to connect to {0} . Reason: {1}", connectionDetails.ServerIP, ex.Message);
            }
        }

        #endregion

        #region Settings

        /// <summary>
        /// This handles the runtime created messages using the .HandleWithExtension 
        /// </summary>
        private static ClientMessageFlowManager clientMessageFlowManager;
        internal static ClientMessageFlowManager MessageFlow
        {
            get 
            { 
                return  clientMessageFlowManager 
                      = clientMessageFlowManager?? new ClientMessageFlowManager();
            }
        }

        /// <summary>
        /// This handles all the actions for various events
        /// e.g. OnConnected, OnDisconnected
        /// </summary>
        private static ClientNetworkActionManager clientActionManager;
        internal static ClientNetworkActionManager ActionManager
        {
            get
            {
                return clientActionManager
                     = clientActionManager ?? new ClientNetworkActionManager();
            }
        }

        /// <summary>
        /// This starts a chain to setup actions or message flow
        /// </summary>
        /// <param name="profileName">The profile of the message setup</param>
        /// <returns>The chain to begin the configuration</returns>
        public static IClientMessageRouter Profile(string profileName)
        {
            return new ClientMessageRouter(profileName);
        }

        /// <summary>
        /// Sends a command to be executed server-side
        /// </summary>
        /// <param name="command">The command</param>
        public static void SendCommand(string command)
        {
            GemNetwork.Client.SendNotification(
                new Notification(command, NotificationType.Command));
        }

        /// <summary>
        /// Sends a notification to the server
        /// </summary>
        /// <param name="message">The notification message</param>
        public static void NotifyServer(string message)
        {
            GemNetwork.Client.SendNotification(
                new Notification(message, NotificationType.Message));
        }

        #endregion
    }
}
