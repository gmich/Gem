using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Infrastructure.Logging
{
    /// <summary>
    /// Registers / Deregisters / Invokes appenders 
    /// </summary>
    public class DebugHost : IDebugHost
    {
        private List<IAppender> appenders;

        public DebugHost()
        {
            appenders = new List<IAppender>();
        }

        #region Register / Deregister

        public void RemoveAll()
        {
            appenders = new List<IAppender>();
        }

        public void RegisterAppender(IAppender appender)
        {
            appenders.Add(appender);
        }

        public void DeregisterAppender(IAppender appender)
        {
            appenders.Remove(appender);
        }

        #endregion

        #region Appending

        public void Info(string message)
        {
            appenders.ForEach(x => x.Info(message));
        }

        public void Info(string message, params object[] args)
        {
            appenders.ForEach(x => x.Info(message, args));
        }

        public void Debug(string message)
        {
            appenders.ForEach(x => x.Debug(message));
        }

        public void Debug(string message, params object[] args)
        {
            appenders.ForEach(x => x.Debug(message, args));
        }

        public void Warn(string message)
        {
            appenders.ForEach(x => x.Warn(message));
        }

        public void Warn(string message, params object[] args)
        {
            appenders.ForEach(x => x.Warn(message, args));
        }

        public void Error(string message)
        {
            appenders.ForEach(x => x.Error(message));
        }

        public void Error(string message, params object[] args)
        {
            appenders.ForEach(x => x.Error(message, args));
        }

        public void Fatal(string message)
        {
            appenders.ForEach(x => x.Fatal(message));
        }

        public void Fatal(string message, params object[] args)
        {
            appenders.ForEach(x => x.Fatal(message, args));
        }

        #endregion

    }
}
