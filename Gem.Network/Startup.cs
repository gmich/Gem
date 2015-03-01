using Gem.Network.Configuration;
using Gem.Network.Utilities.Loggers;
using System;

namespace Gem.Network
{
    public static class Startup
    {

        public static void Setup()
        {
            GemDebugger.Append = new DebugListener();
            GemDebugger.Append.RegisterAppender(new Log4NetWrapper("DebugLogger"));

            var config = new ConfigurationReaderXML();
            config.Load("gem.config");

            Dependencies.Setup(config.Dependencies);
        }

    }
}
