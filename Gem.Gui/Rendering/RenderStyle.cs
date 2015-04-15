using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{
    public struct RenderStyle
    {
        public Texture2D Texture { get; set; }
        public float Transparency { get; set; }
        public float Scale { get; set; }
        public float Layer { get; set; }
        public Color Color { get; set; }
    }

}
