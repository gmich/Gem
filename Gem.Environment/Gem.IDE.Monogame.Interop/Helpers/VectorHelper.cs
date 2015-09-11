using Microsoft.Xna.Framework;

namespace Gem.IDE.MonoGame.Interop.Helpers
{
    internal static class VectorHelper
    {
        public static Vector2 ToVector(this System.Windows.Point point)
        {
            return new Vector2((float)point.X, (float)point.Y);
        }
    }
}