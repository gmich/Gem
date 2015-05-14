using Gem.Gui.Styles;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Gem.Gui.Alignment;

namespace Gem.Gui.Text
{
    public interface IText
    {
        event EventHandler<TextEventArgs> OnTextChanged;

        Region Region { get; }

        Padding Padding { get; set; }

        SpriteFont Font { get; }

        string Value { get; set; }

        ARenderStyle RenderStyle { get; }

        RenderParameters RenderParameters { get; }

        Alignment.AlignmentContext Alignment { get; set; }
    }
}
