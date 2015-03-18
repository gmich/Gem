using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gem.Network.Shooter.Client.Actors
{
using Camera;
using Gem.Network.Client;
using Gem.Network.Events;
using Gem.Network.Shooter.Client.Input;
using Level;
using Scene;
using System;

    public class Bullet : ACollidable
    {

        private Vector2 fallSpeed = new Vector2(0, 20);
        private EffectsManager EffectsManager;
        private readonly Label label;
        private float acceleration;
        private float aliveTime;
        private float timePassed;
        public string Name { get; set; }
        public int SizeX { get { return frameWidth; } }
        public int SizeY { get { return frameHeight; } }

        #region Constructor

        public Bullet(string name, ContentManager content, Vector2 location, Vector2 velocity, float acceleration, float activeTime)
        {
            this.Name = name;                
            this.enabled = true;
            frameWidth = 8;
            frameHeight = 8;
            this.velocity = velocity;
            this.aliveTime = activeTime;
            this.timePassed = 0.0f;
            this.acceleration=acceleration;
            texture = content.Load<Texture2D>(@"bullet");
            CollisionRectangle = new Rectangle(0, 0, 8, 8);
            this.color = Color.DarkRed;
            drawDepth = 0.5f;

            enabled = true;
            tileMap = TileMap.GetInstance();
            Camera = CameraManager.GetInstance();
            this.location = location;
            EffectsManager = EffectsManager.GetInstance();
            this.Transparency = 1.0f;
            var font = content.Load<SpriteFont>(@"font");
            label = new Label(this.Camera.Camera, this.location, new Vector2(+font.MeasureString(name).X / 2 - frameWidth/2, frameHeight / 2 + font.MeasureString(name).Y / 2),
                new FontInfo { Color = Color.Black, Font = font, Text = name }, 100.0f);
        }

        #endregion
        
        public override void Update(GameTime gameTime)
        {
            //HandleVelocity();

            label.ChaseLocation = this.location;
            label.Update(gameTime);

            this.timePassed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            CheckCollision(gameTime);

            if(timePassed>=aliveTime || Collided)
            {
                Enabled = false;
            }
         }

        private const float maxVelocity = 999999f;
        public void HandleVelocity()
        {
            velocity.X = MathHelper.Clamp(Velocity.X + acceleration,
            -maxVelocity, maxVelocity);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            label.Draw(spriteBatch,0.5f);
        }
    }
}