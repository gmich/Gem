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
        private Texture2D background;
        private readonly AIContext context = new AIContext();
        private readonly double timeToBehave = 1.0d;
        private double timePassed = 0.0d;

        public BehaviorTreeVisualizer()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            SetupBehavior();

            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 900;
            graphics.ApplyChanges();
        }

        #region Behavior Related

        public void SetupBehavior()
        {
            int step = 0;

            var walk = new ActionLeaf<AIContext>(
            context => CheckTarget(step = context.InitialStep, context.Target),
            context => CheckTarget(++step, context.Target));
            walk.Name = "walk";

            var unlockDoor = new ActionLeaf<AIContext>(
            context => context.CanUnlock ? BehaviorResult.Success : BehaviorResult.Failure);
            unlockDoor.Name = "unlock door";

            var breakDoor = new ActionLeaf<AIContext>(
           context => BehaviorResult.Success);
            breakDoor.Name = "break door";

            var closeDoor = new ActionLeaf<AIContext>(
            context => BehaviorResult.Failure);
            closeDoor.Name = "close door";

            var checkIfDoorIsCLosed = new PredicateLeaf<AIContext>(
            context => true);
            checkIfDoorIsCLosed.Name = "is door closed?";

            var lockDoor = new ActionLeaf<AIContext>(
            context => BehaviorResult.Success);
            lockDoor.Name = "lock door";

            var openDoor = new Selector<AIContext>(new[] { unlockDoor, breakDoor });
            openDoor.Name = "open door";

            goToRoom = new Sequence<AIContext>(new[] { walk, openDoor, DecorateFor.AlwaysSucceeding(closeDoor), checkIfDoorIsCLosed, lockDoor });
            goToRoom.Name = "go to room";

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
            var linkTexture = Content.Load<Texture2D>(@"Sprites/link");
            var lineTexture = Content.Load<Texture2D>(@"Sprites/line");
            var success = Content.Load<Texture2D>(@"Sprites/success");
            var failure = Content.Load<Texture2D>(@"Sprites/failure");
            var running = Content.Load<Texture2D>(@"Sprites/running");
            var nodeFont = Content.Load<SpriteFont>(@"Fonts/nodeFont");
            background = Content.Load<Texture2D>(@"Sprites/treeBackground");

            visualizer = new TreeVisualizer(nodeTexture, lineTexture, linkTexture, running, success, failure ,nodeFont);
            visualizer.Prepare(new MinimalColorPainter(), goToRoom);

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
            visualizer.Update(gameTime.ElapsedGameTime.TotalSeconds);

            timePassed += gameTime.ElapsedGameTime.TotalSeconds;

            if (timePassed >= timeToBehave)
            {
                timePassed = 0.0d;
                goToRoom.Behave(context);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(new Color(44, 62, 80));

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, new Color(236, 240, 241));
            visualizer.RenderTree(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
