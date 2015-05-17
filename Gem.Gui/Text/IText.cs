using Gem.Gui.Styles;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework.Graphics;
using System;
using Gem.Gui.Alignment;
using Gem.Gui.Transformations;

namespace Gem.Gui.Text
{
    public interface IText : IAlignable, IScalable, ITransformable
    {
        event EventHandler<TextEventArgs> OnTextChanged;

        Padding Padding { get;  }

        SpriteFont Font { get; }

        string Value { get; set; }

        ARenderStyle RenderStyle { get; }

        Alignment.AlignmentContext Alignment { get;  }
    }
}
