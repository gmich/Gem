//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Gem.Network.Shooter.Client
//{
//    public class Label
//    {
//        private readonly RenderInfo renderInfo;
//        private Vector2 chaseLocation;
//        private readonly Vector2 locationOffset;
//        private Vector2 currentLocation;
//        private FontInfo fontInfo;
//        private float Distance
//        {
//            get { return Vector2.Distance((chaseLocation - locationOffset), currentLocation); }
//        }

//        private readonly float acceleration;

//        public Label(Vector2 chaseLocation, Vector2 locationOffset, RenderInfo renderInfo, FontInfo fontinfo, float acceleration)
//        {
//            this.acceleration = acceleration;
//            this.renderInfo = renderInfo;
//            this.chaseLocation = chaseLocation;
//            this.locationOffset = locationOffset;
//            this.renderInfo.Location = this.currentLocation;
//            this.fontInfo = fontinfo;
//        }

//        public void Update(GameTime gameTime)
//        {
//            Vector2 velocity = Vector2.Zero;
//            if (currentLocation != chaseLocation)
//            {
//                velocity = chaseLocation - currentLocation;
//                velocity.Normalize();
//            }

//            currentLocation += velocity * Distance * acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
//        }

//        public void Draw(SpriteBatch spriteBatch)
//        {
//            spriteBatch.Draw(renderInfo.Texture, renderInfo.Rectangle, renderInfo.Color);
//            spriteBatch.DrawString(fontInfo.Font, fontInfo.Text, currentLocation, fontInfo.Color);
//        }
//    }

    
//}