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

    [Flags]
    public enum Direction
    {
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8
    }
    public class Actor : ACollidable
    {
        private const char lifeSymbol = 'o';
        private const float bulletAcceleration = 1000f;
        private Vector2 fallSpeed = new Vector2(0, 20);
        private bool dead = false;
        private int livesRemaining = 999;
        private EffectsManager EffectsManager;
        private readonly INetworkEvent onLocationChange;
        private readonly INetworkEvent onShoot;
        private readonly Label label;
        private readonly Label lifeLabel;
        public double LastUpdated {get; set;}
        private Direction direction;
        private readonly int maxLives;
        public string Name { get; private set; }
        private readonly Vector2 initialLocation;
        SpriteFont font;
        #region Velocity Handler Declarations

        private Vector2 friction = Vector2.Zero;
        private Vector2 groundFriction = new Vector2(20f, 0);
        private Vector2 airFriction = new Vector2(15f, 0);
        private Vector2 accelerationAmount = new Vector2(50f, 0);
        private Vector2 sprintAccelerationAmount = new Vector2(40f, 0);
        private Vector2 currentMaxVelocity = Vector2.Zero;
        private Vector2 sprintMaxVelocity = new Vector2(300f, 0);
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

        public Actor(string name, ContentManager content, Vector2 location, EventManager eventManager, int maxLives)
            : this(name, content, location, maxLives)
        {
            this.livesRemaining = maxLives;
            this.eventManager = eventManager;
            onLocationChange = GemClient.Profile("Shooter")
            .CreateNetworkEventWithRemoteTime
            .AndHandleWith(eventManager, x => new Action<string, float, float, double>(x.SetLocation));

            onShoot = GemClient.Profile("Shooter")
            .CreateNetworkEventWithRemoteTime
            .AndHandleWith(eventManager, x => new Action<string, float, float, float, float, double>(x.Shoot));
        }

        public Actor(string name, ContentManager content, Vector2 location, int maxLives)
        {
            Transparency = 1.0f;
            this.initialLocation = location;
            this.Name = name;
            this.maxLives = maxLives;
            this.livesRemaining = maxLives;
            direction = Direction.Right;
            LastUpdated = 0.0D;
            frameWidth = 48;
            frameHeight = 48;
            texture = content.Load<Texture2D>(@"block");
            CollisionRectangle = new Rectangle(0, 0, 48, 48);
            this.color = Color.DarkBlue;
            drawDepth = 0.825f;

            enabled = true;
            
            tileMap = TileMap.GetInstance();
            Camera = CameraManager.GetInstance();
            this.location = location;
            EffectsManager = EffectsManager.GetInstance();

            font = content.Load<SpriteFont>(@"font");
            label = new Label(this.Camera.Camera, this.location, new Vector2(+font.MeasureString(name).X / 2 - frameWidth/2, frameHeight / 2 + font.MeasureString(name).Y / 2),
                new FontInfo { Color = Color.Black, Font = font, Text =  name }, 20.0f);

            string lifeLabelText = new String(lifeSymbol, LivesRemaining);
            lifeLabel = new Label(this.Camera.Camera, this.location,
                new Vector2(+font.MeasureString(lifeLabelText).X / 2 - frameWidth / 2,
                frameHeight / 2 + font.MeasureString(lifeLabelText).Y / 2 + font.MeasureString(name).Y / 2),
                new FontInfo { Color = Color.DarkRed, Font = font, Text = lifeLabelText }, 20.0f);
        }

        #endregion

        #region Public Methods

        public void HandleInput(GameTime gameTime)
        {
            if (Dead) Revive();

            if (InputHandler.IsKeyDown(Keys.W))
            {
                direction = Direction.Up;
                if (onGround)
                {
                    Jump();
                }
            }
            if (InputHandler.IsKeyDown(Keys.S))
            {
                direction = Direction.Down;
            }
            if (InputHandler.IsKeyDown(Keys.D))
            {
                velocity += accelerationAmount;
                direction = Direction.Right;
                if (InputHandler.IsKeyDown(Keys.W)) direction |= Direction.Up;
                if (InputHandler.IsKeyDown(Keys.S)) direction |= Direction.Down;
            }
            if (InputHandler.IsKeyDown(Keys.A))
            {
                velocity -= accelerationAmount;
                direction = Direction.Left;
                if (InputHandler.IsKeyDown(Keys.W)) direction |= Direction.Up;
                if (InputHandler.IsKeyDown(Keys.S)) direction |= Direction.Down;
            }
            if (InputHandler.IsKeyReleased(Keys.Space))
            {
                var bulletVelocity = Vector2.Zero;

                if (direction.HasFlag(Direction.Up))
                    bulletVelocity -= Vector2.UnitY;
                if (direction.HasFlag(Direction.Down))
                    bulletVelocity += Vector2.UnitY;
                if (direction.HasFlag(Direction.Left))
                    bulletVelocity -= Vector2.UnitX;
                if (direction.HasFlag(Direction.Right))
                    bulletVelocity += Vector2.UnitX;
                bulletVelocity *= bulletAcceleration;

                var bulletLocation = this.location + new Vector2(this.frameWidth / 2, this.frameHeight / 2);
                onShoot.Send(Name, bulletLocation.X, bulletLocation.Y, bulletVelocity.X, bulletVelocity.Y);

                EffectsManager.AddBulletParticle(Name, bulletLocation, bulletVelocity);
            }

            velocity += fallSpeed;
            HandleVelocity();
            CheckCollision(gameTime);
            onLocationChange.Send(Name, location.X, location.Y);
                  
            Camera.Move((float)gameTime.ElapsedGameTime.TotalSeconds, WorldLocation, velocity, accelerationAmount.X);
        }

        public override void Update(GameTime gameTime)
        {
            label.ChaseLocation = this.location;        
            label.Update(gameTime);
            lifeLabel.ChaseLocation = this.location;
            lifeLabel.Update(gameTime);
            this.Transparency = MathHelper.Min(1.0f, Transparency + 0.0005f * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
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
            velocity.Y = -700;
            onAir = true;
        }

        public void Hit(Vector2 otherVelocity)
        {
            LivesRemaining--;
            if (LivesRemaining <= 0)
            {
                dead = true;
                this.LivesRemaining = maxLives;
            }

            this.Transparency = 0.5f;
            lifeLabel.Text = new String(lifeSymbol, LivesRemaining);
            lifeLabel.locationOffset = new Vector2(font.MeasureString(lifeLabel.Text).X / 2 - frameWidth/2, lifeLabel.locationOffset.Y);
            otherVelocity.Normalize();
            this.velocity += otherVelocity * 100.0f;
        }

        public void Revive()
        {
            this.location = initialLocation;
            //this.LivesRemaining = maxLives;
            this.velocity = Vector2.Zero;
            dead = false;
        }

        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            label.Draw(spriteBatch,1.0f);
            lifeLabel.Draw(spriteBatch, 0.7f);
        }
    }
}