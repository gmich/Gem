using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Fluent
{
    public sealed class RenderTextBy
    {
        private static Lazy<TextDrawable> _position = new Lazy<TextDrawable>();
        public static ITextDrawable Position
        {
            get { return _position.Value; }
        }
    }
}
