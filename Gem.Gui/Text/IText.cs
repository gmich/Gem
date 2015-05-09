using Gem.Gui.Core.Styles;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Gui.Text
{
    public interface IText
    {
        event EventHandler<TextEventArgs> OnTextChanged;

        Region Region { get; set;  }

        SpriteFont Font { get;  }

        string Value { get; set; }
        
        IRenderStyle RenderStyle { get; set; }
        
        RenderParameters RenderParameters { get; set; }

        Alignment.AlignmentContext Alignment { get; set; }
    }
}
