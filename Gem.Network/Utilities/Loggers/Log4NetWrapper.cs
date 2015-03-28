namespace Gem.Network.Utilities.Loggers
{
    public class Log4NetWrapper : IAppender
    {
        #region Ctor

        private readonly log4net.ILog logger;

        /// <summary>
        /// Initializes NLog logger by the configuration's reference
        /// </summary>
        /// <param name="loggerReference"></param>
        public Log4NetWrapper(string loggerReference)
        {
            this.logger = log4net.LogManager.GetLogger(loggerReference);
        }

        public Log4NetWrapper(log4net.ILog logger)
        {
            this.logger = logger;
        }

        #endregion

        #region IAppender Implementation

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            logger.InfoFormat(message, args);
        }

        public void Debug(string message)
        {
            logger.Info(message);
        }

        public void Debug(string message, params object[] args)
        {
            logger.DebugFormat(message, args);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Warn(string message, params object[] args)
        {
            logger.WarnFormat(message, args);
        }

        public void Error(string message)
        {
            logger.Info(message);
        }

        public void Error(string message, params object[] args)
        {
            logger.ErrorFormat(message, args);
        }

        public void Fatal(string message)
        {
            logger.Info(message);
        }

        public void Fatal(string message, params object[] args)
        {
            logger.FatalFormat(message, args);
        }

        #endregion
    }
}