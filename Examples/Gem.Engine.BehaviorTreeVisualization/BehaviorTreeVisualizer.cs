using Gem.AI.BehaviorTree;
using Gem.AI.BehaviorTree.Composites;
using Gem.AI.BehaviorTree.Decorators;
using Gem.AI.BehaviorTree.Leaves;
using Gem.AI.BehaviorTree.Visualization;
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
        private IBehaviorNode<AIContext> goToRoom;
        private TreeVisualizer visualizer;

        public BehaviorTreeVisualizer()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            SetupBehavior();
        }

        #region Behavior Related

        public void SetupBehavior()
        {
            int step = 0;

            var walk = new ActionLeaf<AIContext>(
            context => CheckTarget(step = context.InitialStep, context.Target),
            context => CheckTarget(++step, context.Target));

            var unlockDoor = new ActionLeaf<AIContext>(
            context => context.CanUnlock ? BehaviorResult.Success : BehaviorResult.Failure);

            var breakDoor = new ActionLeaf<AIContext>(
           context => BehaviorResult.Success);

            var closeDoor = new ActionLeaf<AIContext>(
            context => BehaviorResult.Failure);

            var openDoor = new Selector<AIContext>(new[] { unlockDoor, breakDoor.TraceAs("BreakDoor") });
            
            goToRoom = new Sequence<AIContext>(new[] { walk, openDoor, DecorateFor.AlwaysSucceeding(closeDoor) });
            
            var aiContext = new AIContext();

            for (int tick = 0; tick < 50; tick++)
            {
                goToRoom.Behave(aiContext);
            }

        }

        internal class AIContext
        {
            public int InitialStep { get; } = 1;
            public int Target { get; set; } = 10;
            public bool CanUnlock { get; } = true;
        }

        private BehaviorResult CheckTarget(int currentStep, int target)
        {
            if (currentStep >= target)
            {
                return BehaviorResult.Success;
            }
            return BehaviorResult.Running;
        }

        #endregion

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var nodeTexture = Content.Load<Texture2D>(@"Sprites/node");

            var nodeFont = Content.Load<SpriteFont>(@"Fonts/nodeFont");
            visualizer = new TreeVisualizer(nodeTexture, nodeTexture, nodeTexture,nodeFont);
            visualizer.Prepare(goToRoom);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            visualizer.RenderTree(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
