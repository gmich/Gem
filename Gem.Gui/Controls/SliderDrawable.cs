using System;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gem.Gui.Alignment;
using Gem.Gui.Utilities;

namespace Gem.Gui.Controls
{
    public class SliderDrawable
    {
        private class SliderItem
        {
            private readonly Texture2D texture;

            public SliderItem(Texture2D texture, Region region)
            {
                this.texture = texture;
                this.Region = region;
            }

            public Texture2D Texture { get { return texture; } }
            public Region Region { get; private set; }

            public void Draw(SpriteBatch batch, RenderParameters renderParams)
            {
                batch.Draw(Texture,
                           Region.Frame,
                           null,
                           renderParams.Color,
                           renderParams.Rotation,
                           renderParams.Origin,
                           renderParams.SpriteEffect,
                           renderParams.Layer);
            }
        }

        #region Fields

        private readonly SliderItem slider;
        private readonly SliderItem border;
        private readonly SliderItem filling;
        private readonly AlignmentContext alignment;

        private Func<double, float> SmoothStep;
        private float destinationX;
        private float previousPercentage;
        private readonly float stepInterpolation = 0.4f;
        private readonly float unreachableDestination = -9999f;
        private readonly float percentageStep;

        #endregion

        #region Ctor

        public SliderDrawable(Texture2D slider, Texture2D border, Texture2D filling, Region borderRegion, AlignmentContext alignment, float percentageStep)
        {
            this.percentageStep = percentageStep;
            this.alignment = alignment;
            this.slider = new SliderItem(slider, new Region(borderRegion.Position.X,
                                                            borderRegion.Position.Y + (borderRegion.Size.Y / 2) - slider.Height / 2,
                                                            slider.Width,
                                                            slider.Height));
            this.border = new SliderItem(border, borderRegion);
            this.filling = new SliderItem(filling, new Region(borderRegion.Position.X, borderRegion.Position.Y, 0, 0));

            this.slider.Region.onPositionChange += (sender, args) => this.filling.Region.Size = new Vector2(this.slider.Region.Position.X - this.border.Region.Position.X,
                                                                                                            this.border.Region.Size.Y);

            SmoothStep = (time) =>
            {
                if (destinationX == unreachableDestination) return 0.0f;
                float distance = (destinationX - this.slider.Region.Position.X);
                return (Math.Abs(distance) <= stepInterpolation) ?
                       distance :
                       distance * MathHelper.Min(1.0f, (float)time * 10);
            };
        }

        #endregion

        #region Properties

        public Region SliderRegion { get { return slider.Region; } }


        public float Percentage
        {
            get
            {
                return GetPercentageByPosition(slider.Region.Position.X);
            }
        }

        #endregion

        #region Private Methods

        private float GetPositionByPercentage(float xPercentage)
        {
            return MathHelper.Clamp(border.Region.Frame.Left + CalculateLocationXPercentage(xPercentage),
                                                                   border.Region.Frame.Left,
                                                                   border.Region.Frame.Right - slider.Region.Size.X);
        }

        private void Move(float xOffset)
        {
            SetLocation(slider.Region.Position.X + xOffset);
        }

        #endregion

        #region Public Methods

        public void Align(Region parent)
        {
            border.Region.Position = alignment.GetTargetRegion(parent, border.Region, Padding.Zero).Position;
            filling.Region.Position = new Vector2(border.Region.Position.X,
                                                  border.Region.Position.Y);
            slider.Region.Position = new Vector2(GetPositionByPercentage(previousPercentage),
                                                 border.Region.Position.Y + (border.Region.Size.Y / 2) - slider.Region.Size.Y / 2);
        }

        public float CalculateLocationXPercentage(float valueX)
        {
            return (valueX * (border.Region.Size.X - slider.Region.Size.X)) / 100;
        }

        public float GetLocationXByPercentage(float percentage)
        {
            return (slider.Region.Position.X * 100) / (border.Region.Size.X);
        }

        public void MoveByPercentage(float xPercentage)
        {
            previousPercentage = xPercentage;
            destinationX = GetPositionByPercentage(xPercentage);
            slider.Region.Position = new Vector2(destinationX, slider.Region.Position.Y);
        }

        public float GetPercentageByPosition(float positionX)
        {
            float startX = positionX - border.Region.Position.X;
            return MathHelper.Clamp((startX * 100) / (border.Region.Size.X - slider.Region.Size.X), 0.0f, 100.0f);
        }

        public void MoveByPercentageSmoothly(float xPercentage)
        {
            destinationX = GetPositionByPercentage(xPercentage);
        }

        public void MoveToLocationSmoothly(float xlocation)
        {
            destinationX = GetValidDestination(xlocation);
        }

        public float GetValidDestination(float xDestination)
        {
            float positionPercentage = GetPercentageByPosition(xDestination);
            return GetPositionByPercentage(
                (float)Math.Round(positionPercentage / percentageStep) * percentageStep);
        }

        public void SetLocation(float xlocation)
        {
            slider.Region.Position = new Vector2(MathHelper.Clamp(xlocation,
                                                                  border.Region.Frame.Left,
                                                                  border.Region.Frame.Right - slider.Region.Size.X),
                                                  slider.Region.Position.Y);
        }

        public void Scale(Vector2 scale)
        {
            slider.Region.Scale(scale);
            filling.Region.Scale(scale);
            border.Region.Scale(scale);
            destinationX = unreachableDestination;
        }

        #endregion

        #region Update / Draw

        public void Update(double deltaTime)
        {
            previousPercentage = GetPercentageByPosition(this.SliderRegion.Position.X);
            float step = SmoothStep(deltaTime);
            if (step != 0.0f)
            {
                Move(step);
            }
        }


        public void Draw(SpriteBatch batch, RenderParameters renderParams)
        {
            border.Draw(batch, renderParams);
            filling.Draw(batch, renderParams);
            slider.Draw(batch, renderParams);
        }
        #endregion

    }
}
