using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Network.Utilities.Loggers
{
    public class DebugListener : IDebugListener
    {
        private List<IAppender> appenders;

        public DebugListener()
        {
            appenders = new List<IAppender>();
        }

        public void Subscribe(IAppender appender)
        {
            appenders.Add(appender);
        }

        public void UnSubscribe(IAppender appender)
        {
            appenders.Remove(appender);
        }

        public void Debug(string message, params object[] args)
        {
            AppendAll(x => x.Debug(message, args));
        }

        public void Error(string message, params object[] args)
        {
            AppendAll(x => x.Error(message, args));
        }

        public void Fatal(string message, params object[] args)
        {
            AppendAll(x => x.Fatal(message, args));
        }

        public void Info(string message, params object[] args)
        {
            AppendAll(x => x.Info(message, args));
        }

        public void Warn(string message, params object[] args)
        {
            AppendAll(x => x.Warn(message, args));
        }

        private void AppendAll(Action<IAppender> appendAction)
        {
            appenders.ForEach(x => appendAction(x));
        }
    }
}
