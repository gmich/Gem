#region Using Declarations

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

#endregion

namespace Gem.Network.Shooter.Client
{
    using Level;
    using Actors;
    using Scene;
    using Gem.Network.Shooter.Client.Input;

    public class ShooterGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private EffectsManager effectsManager;
        private TileMap tileMap;
        private Actor player;
        private readonly string name;
        private EventManager eventManager;

        public ShooterGame(string name)
        {
            this.name = name;
            this.graphics = new GraphicsDeviceManager(this)
            {
                PreferMultiSampling = true,
                PreferredBackBufferWidth = 1000,
                PreferredBackBufferHeight = 500
            };
            Content.RootDirectory = "Content";
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            InputHandler.Initialize();

            tileMap = TileMap.GetInstance();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            IsMouseVisible = true;
            eventManager = new EventManager(Content, name);

            effectsManager = EffectsManager.GetInstance();
            effectsManager.Initialize(Content);
            tileMap.Initialize(Content.Load<Texture2D>(@"block"), 48,48);
            tileMap.Randomize(200, 50);
            player = new Actor(name,Content,new Vector2(1500, 10),eventManager);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            InputHandler.Update(gameTime);
            player.HandleInput(gameTime);
            player.Update(gameTime);
            effectsManager.Update(gameTime);
            eventManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(new Color(71, 196, 241));

            spriteBatch.Begin();

            tileMap.Draw(spriteBatch);
            player.Draw(spriteBatch);
            effectsManager.Draw(spriteBatch);
            eventManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
