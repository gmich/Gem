using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Utilities.Loggers
{
    class ActionAppender : IAppender
    {
        private readonly Action<string> Echo;

        public ActionAppender(Action<string> Echo)
        {
            this.Echo = Echo;
        }
        
        #region Logging

        public void Write(string message)
        {
            Echo(message);
        }

        public void Info(string message, params object[] args)
        {
            Echo(FormatMessage("Info", message, args));
        }

        public void Warn(string message, params object[] args)
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
            return String.Format(prefix + " - " +  message, args);
        }
        #endregion
    }
}
