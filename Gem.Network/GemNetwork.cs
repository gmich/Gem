using Gem.Network.Fluent;
using Gem.Network.Managers;
using Gem.Network.Providers;
using System;

namespace Gem.Network
{
    public static class GemNetwork
    {

        #region Fields
        
        private static MessageFlowManager clientMessageFlowManager = new MessageFlowManager();

        private static ServerConfigurationManager serverConfigurationManager = new ServerConfigurationManager();

        private static ClientConfigurationManager configurationManager = new ClientConfigurationManager();

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
