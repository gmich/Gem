using Microsoft.Xna.Framework.Graphics;
using Gem.IDE.MonoGame.Interop.Controls;

namespace Gem.IDE.MonoGame.Interop.Arguments
{
    /// <summary>
    /// Provides data for the Draw event.
    /// </summary>
    internal sealed class DrawEventArgs : GraphicsDeviceEventArgs
    {
        private readonly DrawingSurface drawingSurface;
	    
        public DrawEventArgs(DrawingSurface drawingSurface, GraphicsDevice graphicsDevice)
			: base(graphicsDevice)
        {
	        this.drawingSurface = drawingSurface;
        }

	    public void InvalidateSurface()
        {
            drawingSurface.Invalidate();
        }
    }
}