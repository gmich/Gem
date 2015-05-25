using Microsoft.Xna.Framework;

namespace Gem.Gui.Utilities
{
    public static class RectangleExtensions
    {
        public static bool Intersects(this Rectangle rect, Point point)
        {
            return (rect.Left <= point.X && rect.Right >= point.X)
                && (rect.Top <= point.Y && rect.Bottom >= point.Y);
        }

        public static Point IntersectionDepth(this Rectangle rect, Point point)
        {
            return new Point(point.X - rect.Left, point.Y - rect.Top);
        }
    }
}
