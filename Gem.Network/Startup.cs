using Gem.Network.Configuration;
using Gem.Network.Protocol;
using Gem.Network.Utilities.Loggers;
using System;

namespace Gem.Network
{
    public static class Startup
    {

        public static void Setup()
        {
            GemNetworkDebugger.Append = new DebugListener();
            GemNetworkDebugger.Append.RegisterAppender(new Log4NetWrapper("DebugLogger"));

            //Setup the default configuration for now
            Dependencies.Setup(new DefaultConfiguration().Load("gem.config"));
        }

    }
}
