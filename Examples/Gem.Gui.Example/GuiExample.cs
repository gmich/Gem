using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Gem.Gui.Fluent;
using Gem.Gui.Alignment;

namespace Gem.Gui.Example
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GuiExample : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GemGui gui;

        public GuiExample()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here
            gui = new GemGui(this);
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

            gui.Fonts.Add("segoe-10",
                          container => container.Load<SpriteFont>(@"segoe-10"));

            var smallButton =
               gui.Button(50, 50, 10, 10)
               .Color(Color.Yellow)
               .Text("Test", 2, 2, gui.Fonts["segoe-10"])
               .TextColor(Color.Red)
               .OnClick((sender, args) => System.Diagnostics.Trace.Write("i clicked a button"));

            var largeButton =
               gui.Button(200, 200, 100, 100)
               .Color(Color.Violet)
               .Text("Test", 2, 2, gui.Fonts["segoe-10"])
               .TextColor(Color.Blue)
               .TextHorizontalAlignment(HorizontalAlignment.Center)
               .TextVerticalAlignment(VerticalAlignment.Center)
               .OnClick((sender, args) => System.Diagnostics.Trace.Write("i clicked a button"));

            gui.AddGuiHost("Main", smallButton, largeButton);
            gui.SwitchTo("Main");

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

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
