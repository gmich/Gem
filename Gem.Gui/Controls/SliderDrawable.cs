using System;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gem.Gui.Alignment;

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

        #endregion

        #region Ctor

        public SliderDrawable(Texture2D slider, Texture2D border, Texture2D filling, Region borderRegion, AlignmentContext alignment)
        {
            this.alignment = alignment;
            this.slider = new SliderItem(slider, new Region(borderRegion.Position.X,
                                                            borderRegion.Position.Y + (borderRegion.Size.Y / 2) - slider.Height / 2,
                                                            slider.Width,
                                                            slider.Height));
            this.border = new SliderItem(border, borderRegion);
            this.filling = new SliderItem(filling, new Region(borderRegion.Position.X, borderRegion.Position.Y, 0, 0));

            this.slider.Region.onPositionChange += (sender, args) => this.filling.Region.Size = new Vector2(this.slider.Region.Position.X - this.border.Region.Position.X,
                                                                                                            this.filling.Region.Size.Y);
            SmoothStep = (time) => 0.0f;
        }

        #endregion

        public void Align(Region parent)
        {
            border.Region.Position = alignment.GetTargetRegion(parent, border.Region, Padding.Zero).Position;
            filling.Region.Position = new Vector2(0,
                                                border.Region.Position.Y);
            slider.Region.Position = new Vector2(0,
                                                 border.Region.Position.Y + (border.Region.Size.Y / 2) - slider.Region.Size.Y / 2);

        }

        public float CalculateLocationXPercentage(float valueX)
        {
            return (valueX * 100) / (border.Region.Size.X);
        }

        public float GetLocationXByPercentage(float percentage)
        {
            return (slider.Region.Position.X * 100) / (border.Region.Size.X);
        }

        public void MoveTo(float xLocation)
        {
            SmoothStep = (time) => (float)(Math.Pow((double)(xLocation - slider.Region.Position.X), 2) * time);
        }

        public void Move(float xOffset)
        {
            SmoothStep = (time) => 0.0f;
            slider.Region.Position = new Vector2(MathHelper.Clamp(slider.Region.Position.X + xOffset,
                                                                   border.Region.Frame.Left,
                                                                   border.Region.Frame.Right - slider.Region.Size.X),
                                                  slider.Region.Position.Y);
        }

        public void MoveByPercentage(float xPercentage)
        {
            SmoothStep = (time) => 0.0f;
            slider.Region.Position = new Vector2(MathHelper.Clamp(CalculateLocationXPercentage(xPercentage),
                                                                   border.Region.Frame.Left,
                                                                   border.Region.Frame.Right - slider.Region.Size.X),
                                                  slider.Region.Position.Y);
        }

        public void Update(double deltaTime)
        {
            float step = SmoothStep(deltaTime);
            if (step != 0.0f)
            {
                Move(step);
            }
        }

        public void Scale(Vector2 scale)
        {
            slider.Region.Scale(scale);
            filling.Region.Scale(scale);
            border.Region.Scale(scale);
        }

        public void Draw(SpriteBatch batch, RenderParameters renderParameters)
        {
            border.Draw(batch, renderParameters);
            filling.Draw(batch, renderParameters);
            slider.Draw(batch, renderParameters);
        }
    }
}
