using Gem.Gui.Styles;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Controls
{
    public class Button : AControl
    {
        public Button(Texture2D texture, Region region, ARenderStyle style, Region parent = null)
            : base(texture, region, style, parent)
        { }
    }
}
