using Gem.Engine.GTerminal.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Engine.Controls.Style
{
    public class StylingRule : IStylingRule
    {
        public Func<int?> WidthGetter { get; set; } = () => null;
        public Func<int?> HeightGetter { get; set; } = () => null;
        public Func<Texture2D> BackgroundImageGetter { get; set; } = () => null;
        public Func<SpriteFont> FontGetter { get; set; } = () => null;
        public Func<Color> BackgroundColorGetter { get; set; } = () => Color.White;
        public Func<Color> TextColorGetter { get; set; } = () => Color.Black;
        public Func<string> TextGetter { get; set; } = () => null;
        public Func<Box> PaddingGetter { get; set; } = () => Box.Empty;
        public Func<BorderBox> BorderGetter { get; set; } = () => BorderBox.Thin;
        public Func<Box> MarginGetter { get; set; } = () => Box.Empty;
        public Func<IAlignment> PositionGetter { get; set; } = () => Alignment.Auto;

        public int? Width { get { return WidthGetter(); } }
        public int? Height { get { return HeightGetter(); } }
        public Texture2D BackgroundImage { get { return BackgroundImageGetter(); } }
        public SpriteFont Font { get { return FontGetter(); } }
        public Color BackgroundColor { get { return BackgroundColorGetter(); } }
        public Color TextColor { get { return TextColorGetter(); } }
        public string Text { get { return TextGetter(); } }
        public Box Padding { get { return PaddingGetter(); } }
        public BorderBox Border { get { return BorderGetter(); } }
        public Box Margin { get { return MarginGetter(); } }
        public IAlignment Position { get { return PositionGetter(); } }
    }
}
