#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Gem.Diagnostics.Console;
using Gem.Engine.Containers;
using Gem.Engine.Console;

#endregion

namespace Gem.Engine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GemEngine : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GemConsole gemConsole;
        private AssetContainer<SpriteFont> fontContainer;
        private AssetContainer<Texture2D> textureContainer;

        public GemEngine()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            fontContainer = new AssetContainer<SpriteFont>(Content);
            textureContainer = new AssetContainer<Texture2D>(Content);
            fontContainer.Add("ConsoleFont", "Fonts/consoleFont");

            //DebugSystem.Initialize(this, "Fonts/consoleFont");
            //DebugSystem.Instance.FpsCounter.Visible = true;
            //DebugSystem.Instance.TimeRuler.Visible = true;
            //DebugSystem.Instance.TimeRuler.ShowLog = true;
            //Register an echo log4net listener
            //DebugSystem.Instance.DebugCommandUI.RegisterEchoListner(new Diagnostics.Logger.LogEchoListener());
            gemConsole = new GemConsole(this, fontContainer["ConsoleFont"]);
            Components.Add(new Input.InputManager(this));
            Components.Add(gemConsole);

            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //DebugSystem.Instance.TimeRuler.StartFrame();
            //DebugSystem.Instance.TimeRuler.BeginMark("Update", Color.Blue);
            // End measuring the Update method
            //DebugSystem.Instance.TimeRuler.EndMark("Update");
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //DebugSystem.Instance.TimeRuler.BeginMark("Draw", Color.CornflowerBlue);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Add methods to measure here

            //DebugSystem.Instance.TimeRuler.EndMark("Draw");

            base.Draw(gameTime);
        }
    }
}
