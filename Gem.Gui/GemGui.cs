using Gem.Gui.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui
{
    public class GemGui : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AssetContainer<Texture2D> textureContainer;
        AssetContainer<SpriteFont> fontContainer;
       
        public GemGui()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureContainer = new AssetContainer<Texture2D>(Content);
            fontContainer = new AssetContainer<SpriteFont>(Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
    
}
