using Gem.Gui.Factories;
using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Styles
{
    public sealed class Style
    {
        private readonly ITextureFactory textureFactory;

        internal Style(ITextureFactory textureFactory)
        {
            this.textureFactory = textureFactory;
        }

        internal ITextureFactory TextureFactory
        { get { return textureFactory; } }

        public static ARenderStyle Transparent
        {
            get
            {
                return new TransparentControlStyle();
            }
        }

        public static ARenderStyle CustomisedTransparent(float focusedAlpha, float hoverAlpha, float defaultAlpha)
        {
            return new TransparentControlStyle(focusedAlpha, hoverAlpha, defaultAlpha);
        }

        public static ARenderStyle ColorMap(Color defaultColor, Color colorMap)
        {
            return new ColorMap(defaultColor, colorMap);
        }

        public static ARenderStyle Bordered
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public static ARenderStyle NoStyle
        {
            get
            {
                return new NoStyle();
            }
        }
    }
}
