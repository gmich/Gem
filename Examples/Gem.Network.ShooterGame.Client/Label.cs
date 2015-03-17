using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gem.Network.Shooter.Client.Camera;

namespace Gem.Network.Shooter.Client
{
    public class Label
    {
        public Vector2 ChaseLocation { get; set; }
        private readonly Vector2 locationOffset;
        private Vector2 currentLocation;
        private FontInfo fontInfo;
        private Camera2D camera;

        public string Text
        {
            get
            {
                return fontInfo.Text;
            }
            set
            {
                fontInfo.Text = value;
            }
        }

        private Vector2 Target
        {
            get
            {
                return ChaseLocation - locationOffset;
            }
        }

        private float Distance
        {
            get { return Vector2.Distance(Target, currentLocation); }
        }

        private readonly float acceleration;

        public Label(Camera2D camera,Vector2 chaseLocation, Vector2 locationOffset, FontInfo fontinfo, float acceleration)
        {
            this.camera = camera;
            this.acceleration = acceleration;
            this.ChaseLocation = chaseLocation;
            this.locationOffset = locationOffset;
            this.currentLocation = chaseLocation;
            this.fontInfo = fontinfo;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 velocity = Vector2.Zero;
            if (currentLocation != Target)
            {
                velocity = Target - currentLocation;
                velocity.Normalize();
            }
            currentLocation += velocity * Distance * acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch,float zoom)
        {
            spriteBatch.DrawString(fontInfo.Font, fontInfo.Text, camera.WorldToScreen(currentLocation), fontInfo.Color, 0.0f, Vector2.Zero, zoom, SpriteEffects.None, 0.0f);
        }
    }

    public class FontInfo
    {
        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public string Text { get; set; }
    }


}