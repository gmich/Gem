using System;

namespace Gem.Infrastructure.Logging
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
