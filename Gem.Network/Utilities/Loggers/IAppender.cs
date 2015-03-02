using System;

namespace Gem.Network.Utilities.Loggers
{
    public interface IAppender
    {
        void Write(string message);

        void Debug(string message, params object[] args);

        void Error(string message, params object[] args);

        void Fatal(string message, params object[] args);

        void Info(string message, params object[] args);

        void Warning(string message, params object[] args);
    }
}
