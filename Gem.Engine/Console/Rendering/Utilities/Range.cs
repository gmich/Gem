using Microsoft.Xna.Framework;

namespace Gem.Console.Rendering.Utilities
{

    public sealed class Range<T>
        where T : struct
    {
        public delegate T BoundariesChecker(T value, T min, T max);

        private readonly T lower;
        private readonly T upper;
        private readonly BoundariesChecker boundariesChecker;

        public Range(T upper, T lower, BoundariesChecker boundariesChecker)
        {
            this.upper = upper;
            this.lower = lower;
            this.boundariesChecker = boundariesChecker;
        }

        public T GetNearest(T value)
        {
            return boundariesChecker(value, upper, lower);
        }

    }

    public static class Range
    {
        public static Range<float> ForFloat(float lower, float upper)
        {
            return new Range<float>(lower, upper, MathHelper.Clamp);
        }
    }

}