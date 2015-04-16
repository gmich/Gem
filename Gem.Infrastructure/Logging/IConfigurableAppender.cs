namespace Gem.Infrastructure.Logging
{
    /// <summary>
    /// An appender that runs a delegate before appending messages.
    /// </summary>
    /// <typeparam name="TAppender">The configurable's appender type</typeparam>
    public interface IConfigurableAppender<TAppender> : IAppender
        where TAppender : IAppender
    {
        AppenderOptions<TAppender> Options { get; set; }
    }
}