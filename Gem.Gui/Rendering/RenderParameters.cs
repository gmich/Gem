using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;
using System;

namespace Gem.Gui.Rendering
{
    /// <summary>
    /// A class that contains all the info the spritebatch needs to render something on the screen.
    /// <remarks>The default values are equivalent to the spritebatch's defaults</remarks>
    /// </summary>
    public class RenderParameters
    {
        public event EventHandler<EventArgs> OnScaleChange;

        public RenderParameters()
        {
            this.AssignDefaultValues();
            this.Color = Color.White;
            this.Scale = Vector2.One;
            this.Origin = Vector2.Zero;
        }

        public Vector2 Origin { get; set; }

        public Color Color { get; set; }

        private Vector2 scale;
        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; OnScaleChanged(); }
        }

        [DefaultValue(1.0f)]
        public float Transparency { get; set; }

        [DefaultValue(1.0f)]
        public float Layer { get; set; }

        [DefaultValue(0.0f)]
        public float Rotation { get; set; }

        [DefaultValue(SpriteEffects.None)]
        public SpriteEffects SpriteEffect { get; set; }

        private void OnScaleChanged()
        {
            var handler = OnScaleChange;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

    }

}
