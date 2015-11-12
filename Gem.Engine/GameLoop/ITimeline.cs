namespace Gem.Engine.GameLoop
{
    public interface ITimeline
    {
        /// <summary>
        /// The delta time in seconds
        /// </summary>
        double DeltaTime { get; }
    }
}
