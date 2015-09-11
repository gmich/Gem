using System;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.IDE.MonoGame.Interop.Arguments
{
    /// <summary>
    /// Arguments used for Device related events.
    /// </summary>
    internal class GraphicsDeviceEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the GraphicsDevice.
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
		/// Initializes a new GraphicsDeviceEventArgs.
        /// </summary>
		/// <param name="graphicsDevice">The GraphicsDevice associated with the event.</param>
		public GraphicsDeviceEventArgs(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }
    }
}
