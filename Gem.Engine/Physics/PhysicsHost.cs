using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Gem.Engine.ScreenSystem;
using Gem.Engine.Containers;
using FarseerPhysics;
using Gem.Engine.Input;
using Microsoft.Xna.Framework.Graphics;
using NullGuard;
using Gem.Diagnostics.DebugViewFarseer;

namespace Gem.Engine.Physics
{
    [NullGuard(ValidationFlags.AllPublicArguments)]
    public class PhysicsHost : Host
    {

        #region Fields and Properties

        public IPhysicsGame Game
        {
            get; set;
        }
        private readonly DebugViewFarseer DebugView;
        private readonly Body HiddenBody;
        private float agentForce;
        private float agentTorque;
        private FixedMouseJoint fixedMouseJoint;
        private Body userAgent;

        public CameraFarseer Camera { get; }
        public bool IsCameraControlled { get; set; }
        public World World { get;}

        #endregion

        public PhysicsHost(ITransition transition,
                           GraphicsDevice device,
                           ContentContainer container)
            : base(transition, device, container)
        {
            World = new World(Vector2.Zero);
            DebugView = new DebugViewFarseer(World);
            DebugView.RemoveFlags(DebugViewFlags.Shape);
            DebugView.RemoveFlags(DebugViewFlags.Joint);
            DebugView.DefaultShapeColor = Color.White;
            DebugView.SleepingShapeColor = Color.LightGray;
            DebugView.LoadContent(Device, Container.Textures.Content);
            Camera = new CameraFarseer(Device);
            HiddenBody = BodyFactory.CreateBody(World, Vector2.Zero);
        }

        #region Private Input Helpers

        private void HandleDebugView(InputManager inputManager)
        {
            if (inputManager.GamePad.IsButtonClicked(Buttons.Start))
            {
                EnableOrDisableFlag(DebugViewFlags.Shape);
                EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
                EnableOrDisableFlag(DebugViewFlags.Joint);
                EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                EnableOrDisableFlag(DebugViewFlags.ContactNormals);
                EnableOrDisableFlag(DebugViewFlags.Controllers);
            }
            if (inputManager.Keyboard.IsKeyClicked(Keys.F1))
            {
                EnableOrDisableFlag(DebugViewFlags.Shape);
            }
            if (inputManager.Keyboard.IsKeyClicked(Keys.F2))
            {
                EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
            }
            if (inputManager.Keyboard.IsKeyClicked(Keys.F3))
            {
                EnableOrDisableFlag(DebugViewFlags.Joint);
            }
            if (inputManager.Keyboard.IsKeyClicked(Keys.F4))
            {
                EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                EnableOrDisableFlag(DebugViewFlags.ContactNormals);
            }
            if (inputManager.Keyboard.IsKeyClicked(Keys.F5))
            {
                EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
            }
            if (inputManager.Keyboard.IsKeyClicked(Keys.F6))
            {
                EnableOrDisableFlag(DebugViewFlags.Controllers);
            }
            if (inputManager.Keyboard.IsKeyClicked(Keys.F7))
            {
                EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
            }
            if (inputManager.Keyboard.IsKeyClicked(Keys.F8))
            {
                EnableOrDisableFlag(DebugViewFlags.AABB);
            }
        }

        private void HandleUserAgent(InputManager inputManager)
        {
            Vector2 force = agentForce * new Vector2(inputManager.GamePad.RightThumpstick.X, -inputManager.GamePad.RightThumpstick.Y);
            float torque = agentTorque * (inputManager.GamePad.RightTriggerPressure - inputManager.GamePad.LeftTriggerPressure);

            userAgent.ApplyForce(force);
            userAgent.ApplyTorque(torque);

            float forceAmount = agentForce * 0.6f;
            force = Vector2.Zero;
            torque = 0;

            if (inputManager.Keyboard.IsKeyPressed(Keys.A))
            {
                force += new Vector2(-forceAmount, 0);
            }
            if (inputManager.Keyboard.IsKeyPressed(Keys.S))
            {
                force += new Vector2(0, forceAmount);
            }
            if (inputManager.Keyboard.IsKeyPressed(Keys.D))
            {
                force += new Vector2(forceAmount, 0);
            }
            if (inputManager.Keyboard.IsKeyPressed(Keys.W))
            {
                force += new Vector2(0, -forceAmount);
            }
            if (inputManager.Keyboard.IsKeyPressed(Keys.Q))
            {
                torque -= agentTorque;
            }
            if (inputManager.Keyboard.IsKeyPressed(Keys.E))
            {
                torque += agentTorque;
            }
            userAgent.ApplyForce(force);
            userAgent.ApplyTorque(torque);
        }

        private void HandleCursor(InputManager inputManager)
        {
            Vector2 position = Camera.ConvertScreenToWorld(inputManager.Mouse.MousePosition.ToVector2());

            if ((inputManager.GamePad.IsButtonPressed(Buttons.A) || inputManager.Mouse.IsLeftButtonPressed())
                && fixedMouseJoint == null)
            {
                Fixture savedFixture = World.TestPoint(position);
                if (savedFixture != null)
                {
                    Body body = savedFixture.Body;
                    fixedMouseJoint = new FixedMouseJoint(body, position);
                    fixedMouseJoint.MaxForce = 1000.0f * body.Mass;
                    World.AddJoint(fixedMouseJoint);
                    body.Awake = true;
                }
            }
            if ((inputManager.GamePad.IsButtonReleased(Buttons.A) || inputManager.Mouse.IsLeftButtonReleased())
                && fixedMouseJoint != null)
            {
                World.RemoveJoint(fixedMouseJoint);
                fixedMouseJoint = null;
            }
            if (fixedMouseJoint != null)
                fixedMouseJoint.WorldAnchorB = position;
        }

        private void HandleCamera(InputManager inputManager, float timeDelta)
        {
            var camMove = Vector2.Zero;

            if (inputManager.Keyboard.IsKeyPressed(Keys.Up))
            {
                camMove.Y -= 10f * timeDelta;
            }
            if (inputManager.Keyboard.IsKeyPressed(Keys.Down))
            {
                camMove.Y += 10f * timeDelta;
            }
            if (inputManager.Keyboard.IsKeyPressed(Keys.Left))
            {
                camMove.X -= 10f * timeDelta;
            }
            if (inputManager.Keyboard.IsKeyPressed(Keys.Right))
            {
                camMove.X += 10f * timeDelta;
            }
            if (inputManager.Keyboard.IsKeyPressed(Keys.PageUp))
            {
                Camera.Zoom += 5f * timeDelta * Camera.Zoom / 20f;
            }
            if (inputManager.Keyboard.IsKeyPressed(Keys.PageDown))
            {
                Camera.Zoom -= 5f * timeDelta * Camera.Zoom / 20f;
            }
            if (camMove != Vector2.Zero)
            {
                Camera.MoveCamera(camMove);
            }
            if (inputManager.Keyboard.IsKeyClicked(Keys.Home))
            {
                Camera.Reset();
            }
        }

        #endregion

        private void EnableOrDisableFlag(DebugViewFlags flag)
        {
            if ((DebugView.Flags & flag) == flag)
            {
                DebugView.RemoveFlags(flag);
            }
            else
            {
                DebugView.AppendFlags(flag);
            }
        }

        public void SetUserAgent(Body agent, float force, float torque)
        {
            userAgent = agent;
            agentForce = force;
            agentTorque = torque;
        }
        
        public override void FixedUpdate(GameTime gameTime)
        {
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

            Camera.Update(gameTime);
            Game.FixedUpdate(gameTime);
        }

        public override void HandleInput(InputManager inputManager, GameTime gameTime)
        {
            HandleDebugView(inputManager);

            if (ScreenManager.Settings.IsMouseVisible)
            {
                HandleCursor(inputManager);
            }
            if (userAgent != null)
            {
                HandleUserAgent(inputManager);
            }
            if (IsCameraControlled)
            {
                HandleCamera(inputManager, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            Game.HandleInput(inputManager, gameTime);
        }

        public override void Initialize() { }

        public override void Draw(SpriteBatch batch)
        {
            Game.Draw(batch);
            DebugView.RenderDebugData(ref Camera.SimProjection, ref Camera.SimView);
        }
    }
}