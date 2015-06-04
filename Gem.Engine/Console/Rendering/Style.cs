using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;
using System;

namespace Gem.Console.Rendering
{
    /// <summary>
    /// A class that contains all the info the spritebatch needs to render something on the screen.
    /// <remarks>The default values are equivalent to the spritebatch's defaults</remarks>
    /// </summary>
    public class Style
    {
        public Style(ITexture texture)
        {
            this.AssignDefaultValues();
            this.Color = Color.White;
            this.Scale = Vector2.One;
            //Double initialization
            //this.Rotation = 0.0f;
            this.Layer = 1.0f;
            this.Transparency = 1.0f;
            this.SpriteEffect = SpriteEffects.None;
            this.Texture = texture;
        }

        public ITexture Texture { get; set; }

        public Color Color { get; set; }

        public Vector2 Scale { get; set; }

        public float Transparency { get; set; }

        public float Layer { get; set; }

        public float Rotation { get; set; }

        public SpriteEffects SpriteEffect { get; set; }

    }

}
