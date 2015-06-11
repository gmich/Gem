namespace Gem.Infrastructure.Logging
{
    /// <summary>
    /// Registers / Deregisters / Uses appenders
    /// </summary>
    public interface IDebugHost : IAppender, IDebugListener 
    {
        void RemoveAppenders();
    }
}
