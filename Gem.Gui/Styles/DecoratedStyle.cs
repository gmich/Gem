using System;
using Gem.Gui.Transformations;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;
using Microsoft.Xna.Framework.Graphics;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;

namespace Gem.Gui.Styles
{
    public sealed class DecoratedStyle : ARenderStyle
    {

        private readonly RenderParameters renderParameters;
        private readonly Sprite sprite;

        public DecoratedStyle(Color color, Texture2D decoration)
        {
            sprite = new Sprite(decoration);
            renderParameters = new RenderParameters();
            renderParameters.Color = color;
            this.AssignDefaultValues();
        }

        #region Properties

        [DefaultValue(0.5f)]
        public float FocusAlpha { get; set; }

        [DefaultValue(0.3f)]
        public float HoverAlpha { get; set; }

        [DefaultValue(0.0f)]
        public float DefaultAlpha { get; set; }

        #endregion

        #region Style

        protected override Func<IRenderable, ITransformation> FocusStyle
        {
            get
            {
                renderParameters.Transparency = FocusAlpha;
                return control => new NoTransformation();
            }
        }
        protected override Func<IRenderable, ITransformation> DefaultStyle
        {
            get
            {
                renderParameters.Transparency = DefaultAlpha;
                return control => new NoTransformation();
            }
        }

        protected override Func<IRenderable, ITransformation> HoverStyle
        {
            get
            {
                renderParameters.Transparency = HoverAlpha;
                return control => new NoTransformation();
            }
        }

        protected override Func<IRenderable, ITransformation> ClickedStyle
        {
            get
            {
                renderParameters.Transparency = DefaultAlpha;
                return control => new NoTransformation();
            }
        }

        #endregion

        public override void Render(IRenderable renderable, SpriteBatch batch)
        {
            batch.Draw(sprite.Texture,
                       renderable.Region.Frame,
                       sprite.SourceRectangle,
                       renderParameters.Color * renderParameters.Transparency,
                       renderParameters.Rotation,
                       renderable.RenderParameters.Origin,
                       renderParameters.SpriteEffect,
                       renderParameters.Layer);
        }

    }
}
