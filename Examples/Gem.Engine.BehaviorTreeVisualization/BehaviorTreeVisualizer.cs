using Gem.Engine.AI.BehaviorTree;
using Gem.Engine.AI.BehaviorTree.Composites;
using Gem.Engine.AI.BehaviorTree.Decorators;
using Gem.Engine.AI.BehaviorTree.Leaves;
using Gem.Engine.AI.BehaviorTree.Visualization;
using Gem.DrawingSystem;
using Gem.Engine.BehaviorTreeVisualization.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gem.Engine.BehaviorTreeVisualization
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BehaviorTreeVisualizer : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private TreeVisualizer visualizer;
        private Texture2D background;
        private RenderTargetRenderer renderer;
        private Level level;

        public BehaviorTreeVisualizer()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 715;
            graphics.PreferredBackBufferWidth = 1420;
            graphics.ApplyChanges();
        }



        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            level = new Level(Content, 64, 64);

            var nodeTexture = Content.Load<Texture2D>(@"Sprites/node");
            var linkTexture = Content.Load<Texture2D>(@"Sprites/link");
            var lineTexture = Content.Load<Texture2D>(@"Sprites/line");
            var success = Content.Load<Texture2D>(@"Sprites/success");
            var failure = Content.Load<Texture2D>(@"Sprites/failure");
            var running = Content.Load<Texture2D>(@"Sprites/running");
            var nodeFont = Content.Load<SpriteFont>(@"Fonts/nodeFont");
            background = Content.Load<Texture2D>(@"Sprites/treeBackground");

            visualizer = new TreeVisualizer(nodeTexture, lineTexture, linkTexture, running, success, failure, nodeFont);
            level.Context.onBehaviorChanged += (sender, args) => ConfigureTreeVisualizer((sender as BehaviorContext).Behavior);
            ConfigureTreeVisualizer(level.Context.Behavior);

        }

        private void ConfigureTreeVisualizer<AIContext>(IBehaviorNode<AIContext> behavior)
        {
            visualizer.Prepare(new MinimalColorPainter(), level.Context.Behavior);
            renderer = new RenderTargetRenderer(batch =>
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                visualizer.RenderTree(spriteBatch);
                spriteBatch.End();
            },
               GraphicsDevice,
               new Vector2(GraphicsDevice.Viewport.Width*2, visualizer.TreeSize.Y));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            level.Update(gameTime.ElapsedGameTime.TotalSeconds);
            visualizer.Update(gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(new Color(44, 62, 80));

            renderer.Render(spriteBatch);
            spriteBatch.Begin();
            spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, new Color(236, 240, 241));
            spriteBatch.Draw(renderer.Target, new Vector2(-220, -15), Color.White);
            level.Draw(spriteBatch, new Vector2(60, 645));
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
