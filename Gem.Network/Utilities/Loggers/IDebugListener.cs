using System;

namespace Gem.Network.Utilities.Loggers
{
    public interface IDebugListener : IAppender
    {
        void Subscribe(IAppender appender);
        void UnSubscribe(IAppender appender);
    }
}
