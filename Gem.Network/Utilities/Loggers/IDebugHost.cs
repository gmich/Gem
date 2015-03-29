namespace Gem.Network.Utilities.Loggers
{
    /// <summary>
    /// Registers / Deregisters / Uses appenders
    /// </summary>
    public interface IDebugHost : IAppender, IDebugListener 
    {
        void RemoveAll();
    }
}
