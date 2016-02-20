using Microsoft.Xna.Framework;
using System;

namespace Gem.Engine.GameLoop
{
    public interface ITimeline
    {
        /// <summary>
        /// The delta time in seconds
        /// </summary>
        TimeSpan DeltaTime { get; }

    }
}
