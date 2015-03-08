using Gem.Network.Commands;
using Gem.Network.Fluent;
using Gem.Network.Managers;
using Gem.Network.Providers;
using Gem.Network.Server;
using Gem.Network.Utilities.Loggers;
using System;

namespace Gem.Network
{
    public static class GemNetwork
    {

        //Predefined package types
        internal static readonly byte NotificationByte = (byte)1;

        static GemNetwork()
        {
            Startup.Setup();
            clientMessageFlowManager = new ClientMessageFlowManager();
            serverConfigurationManager = new ServerConfigurationManager();
            Client = new Peer();
            Server = new NetworkServer(GemNetworkDebugger.Echo);
            commander = new ServerCommandHost(Server);
            predefinedMessageFlowManager = new ClientPredefinedMessageFlowManager();
            clientActionManager = new ClientNetworkActionManager();   
        }    

        #region Fields

        private static ClientPredefinedMessageFlowManager predefinedMessageFlowManager;

        private static ClientMessageFlowManager clientMessageFlowManager;

        private static ServerConfigurationManager serverConfigurationManager;

        private static ClientNetworkActionManager clientActionManager;

        internal static ICommandHost commander;

        internal static IClient Client;

        internal static IServer Server;

        internal static int dynamicMessagesCreated = 2;

        #endregion


        #region Properties

        internal static string ActiveProfile
        {
            get;
            set;
        }

        internal static ICommandHost Commander
        {
            get
            {
                return commander;
            }
        }

        internal static ClientMessageFlowManager ClientMessageFlow
        {
            get
            {
                return clientMessageFlowManager;
            }
        }

        internal static ClientNetworkActionManager ClientActionManager
        {
            get
            {
                return clientActionManager;
            }
        }
        
        internal static ServerConfigurationManager ServerConfiguration
        {
            get
            {
                return serverConfigurationManager;
            }
        }



        #endregion

    }
}
