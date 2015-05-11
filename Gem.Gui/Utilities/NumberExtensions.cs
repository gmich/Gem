using Microsoft.Xna.Framework;

namespace Gem.Gui.Utilities
{
    public static class NumberExtensions
    {
        public static float Approach(this float current, float target, float step)
        {
            return (current > target) ?
                MathHelper.Max(target, current - step) :
                MathHelper.Min(target, current + step);
        }
    }
}
