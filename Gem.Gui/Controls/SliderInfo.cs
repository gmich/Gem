using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Gem.Gui.Controls
{
    public struct SliderInfo
    {
        private readonly float minValue;
        private readonly float maxValue;
        private readonly float step;
        private readonly float stepsUntilFull;
        private float position;

        public SliderInfo(float minValue, float maxValue, int stepsUntilFull, float initialPosition)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.stepsUntilFull = stepsUntilFull;
            this.step = (maxValue - minValue) / stepsUntilFull;
            position = MathHelper.Clamp(initialPosition, minValue, maxValue);
        }

        public SliderInfo(float minValue, float maxValue, float step, float initialPosition)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.step = step;
            this.stepsUntilFull = (maxValue - minValue) / step;
            position = MathHelper.Clamp(initialPosition, minValue, maxValue);
        }

        public float Position
        {
            get { return position; }
            set { position = MathHelper.Clamp(value, minValue, maxValue); }
        }

        public float PositionPercent { get { return CalculatePercentage(this.position); } }

        public float Min { get { return minValue; } }

        public float Max { get { return maxValue; } }

        public float Step { get { return step; } }

        public void Move(float value)
        {
            Position += value;
        }

        public float CalculatePercentage(float value)
        {
            return (value * 100) / (Max - Min);
        }
    }
}
