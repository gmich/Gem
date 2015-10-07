using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using Gem.Engine.Configuration;
using Gem.Engine.Containers;
using Gem.Engine.ScreenSystem;

namespace Gem.Engine.Physics
{
    public class GameBase : Game
    {
        public event EventHandler<EventArgs> onInitialize;
        public event EventHandler<ContentManager> onContentLoad;
        public event EventHandler<EventArgs> onContentUnload;
        public event EventHandler<SpriteBatch> onDraw;
        public event EventHandler<GameTime> onUpdate;

        public GraphicsDeviceManager GraphicsManager { get; }        
        public SpriteBatch SpriteBatch { get; private set; }
        public ScreenManager ScreenManager { get; }
        public ContentContainer Container { get; private set; }
        
        public GameBase(Settings settings)
        {
            var inputManager = new Input.InputManager(this);
            Components.Add(inputManager);
            GraphicsManager = new GraphicsDeviceManager(this);
            ScreenManager = new ScreenManager(this, inputManager, settings, (batch) => onDraw?.Invoke(this,batch));
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            onInitialize?.Invoke(this, EventArgs.Empty);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Container = new ContentContainer(Content);
            onContentLoad?.Invoke(this, Content);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            onContentUnload?.Invoke(this, EventArgs.Empty);
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            onUpdate?.Invoke(this, gameTime);
            base.Update(gameTime);
        }

    }
}