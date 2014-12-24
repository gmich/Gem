using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroGameEngine.Diagnostics.Logger
{
    using Console;

    public class LogEchoListener : IDebugEchoListner
    {
        #region Fields

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ConsoleEchoLogger");

        #endregion


        public void Echo(DebugCommandMessage messageType, string text)
        {
            switch (messageType)
            {
                case DebugCommandMessage.Standard:
                    log.Info(text);
                    break;
                case DebugCommandMessage.Warning:
                    log.Warn(text);
                    break;
                case DebugCommandMessage.Error:
                    log.Error(text);
                    break;
            }
        }
  
    }
}
