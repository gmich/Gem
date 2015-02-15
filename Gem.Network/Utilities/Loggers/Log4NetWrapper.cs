using System;

namespace Gem.Network.Utilities.Loggers
{
    public class Log4NetWrapper : IAppender
    {
        private readonly log4net.ILog log;

        public Log4NetWrapper(string logger) 
        {
            log = log4net.LogManager.GetLogger(logger);
        }

        #region Logging

        public void Info(string message, params object[] args)
        {
            log.InfoFormat(message,args);
        }

        public void Warn(string message, params object[] args)
        {
            log.WarnFormat(message, args);
        }

        public void Error(string message, params object[] args)
        {
            log.ErrorFormat(message, args);
        }
        
        public void Debug(string message, params object[] args)
        {
            log.DebugFormat(message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            log.FatalFormat(message, args);
        }
         
        #endregion

    }
}

