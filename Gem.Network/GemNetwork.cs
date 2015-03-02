using Gem.Network.Fluent;
using Gem.Network.Managers;
using Gem.Network.Providers;
using Gem.Network.Server;
using System;

namespace Gem.Network
{
    public static class GemNetwork
    {

        static GemNetwork()
        {
            Startup.Setup();
            clientMessageFlowManager = new MessageFlowManager();
            serverConfigurationManager = new ServerConfigurationManager();
            configurationManager = new ClientConfigurationManager();
            Client = new Peer();
            Server = new NetworkServer(Console.WriteLine);
        }    

        #region Fields
        
        private static MessageFlowManager clientMessageFlowManager;

        private static ServerConfigurationManager serverConfigurationManager;

        private static ClientConfigurationManager configurationManager;

        private static NetworkActionManager clientActionManager;

        internal static IClient Client;

        internal static IServer Server;

        internal static int profilesInvoked = 0;

        #endregion


        #region Properties

        public static string ActiveProfile
        {
            get;
            set;
        }

        internal static MessageFlowManager ClientMessageFlow
        {
            get
            {
                return clientMessageFlowManager;
            }
        }

        internal static NetworkActionManager ClientActionManager
        {
            get
            {
                return clientActionManager;
            }
        }

        internal static ClientConfigurationManager ClientConfiguration
        {
            get
            {
                return configurationManager;
            }
        }

        internal static ServerConfigurationManager ServerConfiguration
        {
            get
            {
                return serverConfigurationManager;
            }
        }

        public static ProfileRouter Profile(string profileName)
        {
            profilesInvoked++;
            return new ProfileRouter(profileName);
        }

        #endregion

    }
}
