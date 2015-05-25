using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Gui.Controls
{
    public class SliderInfo
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

        #region Events

        internal event EventHandler<float> OnPositionChanged;

        private void OnPositionChange()
        {
            var handler = OnPositionChanged;
            if (handler != null)
            {
                handler(this, Position);
            }
        }

        #endregion 

        public float Position
        {
            get { return position; }
            set
            {
                position = MathHelper.Clamp(value, minValue, maxValue);
                OnPositionChange();
            }
        }

        internal float PositionPercent { get { return CalculatePercentage(this.position); } }

        public float Min { get { return minValue; } }

        public float Max { get { return maxValue; } }

        public float Step { get { return step; } }

        internal void Move(float value)
        {
            Position += value;
        }

        internal void SetPositionByPercentage(float percentage)
        {
            Position = Min + ((percentage * (Max - Min)) / 100);
        }

        internal float CalculatePercentage(float value)
        {
            return ((value - Min) * 100) / (Max - Min);
        }
    }
}
