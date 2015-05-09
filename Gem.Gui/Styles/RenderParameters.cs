using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;

namespace Gem.Gui.Core.Styles
{
    /// <summary>
    /// A class that contains all the info the spritebatch needs to render something on the screen.
    /// <remarks>The default values are equivalent to the spritebatch's defaults</remarks>
    /// </summary>
    public class RenderParameters
    {
        public RenderParameters()
        {
            this.AssignDefaultValues();
            this.Color = Color.White;
            this.Scale = Vector2.One;
        }

        public Color Color { get; set; }

        public Vector2 Scale { get; set; }
        
        [DefaultValue(1.0f)]
        public float Transparency { get; set; }
        
        [DefaultValue(1.0f)]
        public float Layer { get; set; }

        [DefaultValue(1.0f)]
        public float Rotation { get; set; }

        [DefaultValue(SpriteEffects.None)]
        public SpriteEffects SpriteEffect { get; set; }

    }

}
