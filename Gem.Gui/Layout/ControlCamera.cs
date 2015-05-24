using Microsoft.Xna.Framework;
using Gem.Gui.Rendering;

namespace Gem.Gui.Layout
{

    internal sealed class ControlCamera
    {

        #region Fields

        private readonly Region viewRegion;
        private readonly Region virtualRegion;

        #endregion

        #region Ctor

        public ControlCamera(Vector2 initialPosition, Rectangle viewport, Rectangle virtualViewport)
        {
            Position = initialPosition;
            viewRegion = Region.FromRectangle(viewport);
            virtualRegion = Region.FromRectangle(virtualViewport);
        }

        #endregion

        #region Properties

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            private set
            {
                //adjust within the virtual region
                position = new Vector2(x: MathHelper.Clamp(value.X, virtualRegion.Frame.Left, virtualRegion.Frame.Right - viewRegion.Size.X),
                                       y: MathHelper.Clamp(value.Y, virtualRegion.Frame.Top, virtualRegion.Frame.Bottom - viewRegion.Size.Y));
            }
        }

        public Rectangle Viewport
        {
            get { return viewRegion.Frame; }
        }

        public Rectangle VirtualViewport
        {
            get { return virtualRegion.Frame; }
        }

        #endregion

        #region Public Methods

        public void Move(Vector2 offset)
        {
            Position += offset;
        }

        public void Scale(Vector2 scale)
        {
            viewRegion.Scale(scale);
            virtualRegion.Scale(scale);
        }

        #endregion


    }
}
