using Gem.Gui.Controls;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Rendering
{
    public class ControlDrawable : AControlDrawable
    {
        public ControlDrawable(SpriteBatch batch) : base(batch) { }

        public override void Render(AControl control)
        {
            batch.Draw(control.Sprite.Texture,
                       control.Region.Frame,
                       control.RenderParameters.Color);
        }
    }
}
