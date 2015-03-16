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

    public class Actor : ACollidable
    {
        private Vector2 fallSpeed = new Vector2(0, 20);
        private bool dead = false;
        private int livesRemaining = 999;
        private EffectsManager EffectsManager;
        private readonly string name;
        private readonly INetworkEvent onLocationChange;
        private readonly Label label;
        public double LastUpdated {get; set;}

        #region Velocity Handler Declarations

        private Vector2 friction = Vector2.Zero;
        private Vector2 groundFriction = new Vector2(20f, 0);
        private Vector2 airFriction = new Vector2(15f, 0);
        private Vector2 accelerationAmount = new Vector2(50f, 0);
        private Vector2 sprintAccelerationAmount = new Vector2(40f, 0);
        private Vector2 currentMaxVelocity = Vector2.Zero;
        private Vector2 sprintMaxVelocity = new Vector2(250f, 0);
        private bool onAir;
        private readonly EventManager eventManager;
        #endregion

        public bool Dead
        {
            get { return dead; }
        }

        public float Friction
        {
            get { return friction.X; }
        }

        public bool IsJumping
        {
            get { return onAir; }
        }

        public float VelocityY
        {
            get { return velocity.Y; }
        }

        public int LivesRemaining
        {
            get { return livesRemaining; }
            set { livesRemaining = value; }
        }

        #region Constructor

        public Actor(string name, ContentManager content, Vector2 location,EventManager eventManager)
            : this(name , content, location)
        {
            this.eventManager = eventManager;
            onLocationChange = GemClient.Profile("Shooter")
            .CreateNetworkEventWithRemoteTime
            .AndHandleWith(eventManager, x => new Action<string, float, float,double>(x.SetLocation));

        }

        public Actor(string name, ContentManager content,Vector2 location)
        {
            LastUpdated = 0.0D;
            this.name = name;
            frameWidth = 48;
            frameHeight = 48;
            texture = content.Load<Texture2D>(@"block");
            CollisionRectangle = new Rectangle(0, 0, 48, 48);
            this.color = Color.DarkBlue;
            drawDepth = 0.825f;

            enabled = true;


            tileMap = TileMap.GetInstance();
            Camera = CameraManager.GetInstance();
            livesRemaining = 999;
            worldLocation = location;
            EffectsManager = EffectsManager.GetInstance();

            var font = content.Load<SpriteFont>(@"font");
            label = new Label(this.Camera.Camera, this.worldLocation, new Vector2(+font.MeasureString(name).X / 2 - frameWidth/2, frameHeight / 2 + font.MeasureString(name).Y / 2),
                new FontInfo { Color = Color.Black, Font = font, Text = name }, 20.0f);
        }

        #endregion

        #region Public Methods

        public void HandleInput(GameTime gameTime)
        {
            if (Dead) return;

            if (InputHandler.IsKeyDown(Keys.D))
            {
                velocity += accelerationAmount;
            }
            if (InputHandler.IsKeyDown(Keys.A))
            {
                velocity -= accelerationAmount;
            }
            if (InputHandler.IsKeyDown(Keys.W))
            {
                if (onGround)
                {

                    Jump();
                }
            }

            velocity += fallSpeed;
            HandleVelocity();
            CheckCollision(gameTime);
            onLocationChange.Send(this.name, worldLocation.X, worldLocation.Y);
           // EffectsManager.AddBulletParticle(worldLocation + offSet, bulletDirection);   EffectsManager.AddBulletParticle(worldLocation + offSet, bulletDirection);
                  
            Camera.Move((float)gameTime.ElapsedGameTime.TotalSeconds, WorldLocation, velocity, accelerationAmount.X);
        }

        public override void Update(GameTime gameTime)
        {

            label.ChaseLocation = this.worldLocation;
            label.Update(gameTime);

          
         }

        public void HandleVelocity()
        {
            currentMaxVelocity = sprintMaxVelocity;
            accelerationAmount = sprintAccelerationAmount;

            if (onGround)
            {
                friction = groundFriction;
            }
            else if (onAir)
            {
                friction = airFriction;
            }
            if (velocity.X > 0)
            {
                velocity.X = MathHelper.Clamp(Velocity.X - friction.X / 2,
                    0, +currentMaxVelocity.X);
            }
            else
            {
                velocity.X = MathHelper.Clamp(Velocity.X + friction.X / 2,
                    -currentMaxVelocity.X, 0);
            }
        }

    

        public void Jump()
        {
            velocity.Y = -600;
            onAir = true;
        }

        public void Hit()
        {
            LivesRemaining--;
            velocity.X = 0;
            dead = true;
        }

        public void Revive()
        {
            dead = false;
        }

        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            label.Draw(spriteBatch);
        }
    }
}