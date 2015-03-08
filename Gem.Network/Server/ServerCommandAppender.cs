using Gem.Network.Utilities.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using Gem.Network.Messages;

namespace Gem.Network.Server
{
    public class ServerCommandAppender : IAppender
    {

        public Action<string> Echo;
        
        public ServerCommandAppender(IServer server)
        {
            Echo = msg =>
            {
                server.NotifyAll(msg);
            };
        }

        public void Write(string message)
        {
            Echo(message);
        }

        public void Info(string message, params object[] args)
        {
            Echo(FormatMessage("Info", message, args));
        }

        public void Warning(string message, params object[] args)
        {
            Echo(FormatMessage("Warn", message, args));
        }

        public void Error(string message, params object[] args)
        {
            Echo(FormatMessage("Error", message, args));
        }

        public void Debug(string message, params object[] args)
        {
            Echo(FormatMessage("Debug", message, args));
        }

        public void Fatal(string message, params object[] args)
        {
            Echo(FormatMessage("Fatal", message, args));
        }

        private string FormatMessage(string prefix, string message, params object[] args)
        {
            return String.Format(prefix + " - " + message, args);
        }
    }
}
