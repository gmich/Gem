using Gem.Network.Configuration;
using Gem.Network.Utilities.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network
{
    public static class Startup
    {
        static Startup()
        {
            Debugger.Append = new DebugListener();
            Debugger.Append.RegisterAppender(new Log4NetWrapper("DebugLogger"));

            var config = new ConfigurationReaderXML();
            config.Load("gem.config");

            Dependencies.Setup(config.Dependencies);
        }
    }
}
