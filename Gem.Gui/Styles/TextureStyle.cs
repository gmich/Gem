using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{
    public class RenderStyle
    {
        /// <summary>
        /// Initializes a new instance of RenderStyle. The default values are equivalent with spritebatch's defaults
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="color"></param>
        /// <param name="transparency"></param>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        /// <param name="spriteEffects"></param>
        public RenderStyle(float layer,
                           Color? color = null,
                           float transparency = 1.0f,
                           float scale = 1.0f,
                           float rotation = 1.0f,
                           SpriteEffects spriteEffects= SpriteEffects.None)
        {
            this.Layer = layer;
            this.Color = color ?? Color.White;
            this.Transparency = transparency;
            this.Scale = scale;
            this.Rotation = rotation;
            this.SpriteEffects = spriteEffects;
        }
        public float Transparency { get; set; }
        public float Scale { get; set; }
        public float Layer { get; set; }
        public float Rotation { get; set; }
        public Color Color { get; set; }
        public SpriteEffects SpriteEffects { get; set; }
    }

}
