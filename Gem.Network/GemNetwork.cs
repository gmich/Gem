using Gem.Network.Fluent;
using Gem.Network.Managers;
using Gem.Network.Providers;
using System;

namespace Gem.Network
{
    public static class GemNetwork
    {

        public static string ActiveProfile { get; set;}

        private static MessageFlowManager clientMessageFlowManager = new MessageFlowManager();

        internal static int profilesInvoked = 0;

        internal static MessageFlowManager ClientMessageFlowManager
        {
            get
            {
                return clientMessageFlowManager;
            }
        }

        public static IMessageRouter Profile(string profileName)
        {
            profilesInvoked++;
            return new MessageRouter(profileName);
        }
    }
}
