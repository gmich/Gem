using System;

namespace Gem.Network.Utilities.Loggers
{
    /// <summary>
    /// Registers / Deregisters appenders
    /// </summary>
    public interface IDebugListener
    {
        void RegisterAppender(IAppender appender);
        void DeregisterAppender(IAppender appender);
    }
}
