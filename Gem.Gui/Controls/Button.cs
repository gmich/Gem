using Gem.Gui.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Controls
{
    public class Button : AControl
    {
        private readonly Texture2D texture;

        public Button(Texture2D texture, Region region):base(region)
        { }
    }
}
