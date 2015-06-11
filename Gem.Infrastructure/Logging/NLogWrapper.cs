namespace Gem.Infrastructure.Logging
{
    public class NLogWrapper : IAppender
    {
        #region Ctor

        private readonly NLog.Logger logger;

        /// <summary>
        /// Initializes NLog logger by the configuration's reference
        /// </summary>
        /// <param name="loggerReference"></param>
        public NLogWrapper(string loggerReference)
        {
            this.logger = NLog.LogManager.GetLogger(loggerReference);
        }

        public NLogWrapper(NLog.Logger logger)
        {
            this.logger = logger;
        }

        #endregion

        #region IAppender Implementation

        public void Message(string message)
        {
            Info(message);
        }

        public void Message(string message, params object[] args)
        {
            Info(message, args);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            logger.Info(message,args);
        }

        public void Debug(string message)
        {
            logger.Info(message);
        }

        public void Debug(string message, params object[] args)
        {
            logger.Info(message, args);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Warn(string message, params object[] args)
        {
            logger.Warn(message, args);
        }

        public void Error(string message)
        {
            logger.Info(message);
        }

        public void Error(string message, params object[] args)
        {
            logger.Error(message, args);
        }

        public void Fatal(string message)
        {
            logger.Info(message);
        }

        public void Fatal(string message, params object[] args)
        {
            logger.Fatal(message, args);
        }
        
        #endregion
    }
}