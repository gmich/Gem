using Gem.Gui.Text;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Rendering
{
    public class TextDrawable : ATextDrawable
    {
        public TextDrawable(SpriteBatch batch) : base(batch) { }

        public override void Render(IText text)
        {
            batch.DrawString(text.Font,
                             text.Value,
                             text.Region.Position,
                             text.RenderParameters.Color);
        }
    }
}
