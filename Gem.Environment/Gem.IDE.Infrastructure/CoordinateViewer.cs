using Gem.Engine.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Gem.IDE.Infrastructure
{
    public class CoordinateViewer
    {
        private readonly GraphicsDevice device;
        private readonly Color color;
        private readonly int viewportHeight;
        private readonly int viewportWidth;
        private IShape XLine;
        private IShape YLine;

        public CoordinateViewer(GraphicsDevice device,Color color,int viewportWidth, int viewportHeight)
        {
            this.device = device;
            this.viewportHeight = viewportHeight;
            this.viewportWidth = viewportWidth;
            this.color = color;
        }

        public void Disable()
        {
            XLine = null;
            YLine = null;
        }

        public void Set(int x,int y)
        {
            XLine = new FixedBoundsShape(
                            new List<Vector2> { new Vector2(x, 0), new Vector2(x, viewportHeight) },
                            0,
                            0,
                            viewportWidth,
                            viewportHeight, 
                            color,
                            device);

            YLine = new FixedBoundsShape(
                new List<Vector2> { new Vector2(0, y), new Vector2(viewportWidth, y) },
                0,
                0,
                viewportWidth,
                viewportHeight,
                color,
                device);
        }

        public void Draw()
        {
            XLine?.Draw();
            YLine?.Draw();
        }


    }
}
