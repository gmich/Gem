using System;
using Gem.Gui.Controls;
using Gem.Gui.Transformations;
using Gem.Gui.Utilities;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;
using Microsoft.Xna.Framework.Graphics;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;

namespace Gem.Gui.Styles
{
    public sealed class ColorMap : ARenderStyle
    {

        private readonly Color defaultColor;
        private readonly Color colorMap;

        public ColorMap(Color defaultColor, Color colorMap)
        {
            this.defaultColor = defaultColor;
            this.colorMap = colorMap;
        }

        #region Style

        protected override Func<IRenderable, ITransformation> FocusStyle
        {
            get
            {
                return renderable =>
                    new OneTimeTransformation(control => renderable.RenderParameters.Color = colorMap);
            }
        }
        protected override Func<IRenderable, ITransformation> DefaultStyle
        {
            get
            {

                return renderable =>
                    new OneTimeTransformation(control => renderable.RenderParameters.Color = defaultColor);

            }
        }

        protected override Func<IRenderable, ITransformation> HoverStyle
        {
            get
            {
                return renderable =>
                    new OneTimeTransformation(control =>
                        renderable.RenderParameters.Color = Color.Lerp(defaultColor, colorMap, 0.7f));
            }
        }

        protected override Func<IRenderable, ITransformation> ClickedStyle
        {
            get
            {
                return renderable =>
                    new OneTimeTransformation(control =>
                        renderable.RenderParameters.Color = Color.Lerp(defaultColor, colorMap, 0.5f));
            }
        }

        #endregion

        public override void Render(IRenderable renderable, SpriteBatch batch)
        {
            return;          
        }

    }
}
