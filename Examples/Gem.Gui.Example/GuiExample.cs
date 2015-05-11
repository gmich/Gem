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
            this.IsMouseVisible = true;
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

            gui.Fonts.Add("segoe-10", @"Fonts/segoe-10");

            var smallButton =
               gui.Button(50, 50, 100, 100)
               .Color(Color.White)
               .Text("Test", 0, 0, gui.Fonts["segoe-10"])
               .TextColor(Color.Black)
               .TextHorizontalAlignment(HorizontalAlignment.Center)
               .TextVerticalAlignment(VerticalAlignment.Bottom)
               .OnClick((sender, args) => System.Diagnostics.Trace.Write("i clicked a button"));

            smallButton.Events.GotMouseCapture += (sender, args) => System.Console.WriteLine("GotMouseCapture");
            smallButton.Events.LostMouseCapture += (sender, args) => System.Console.WriteLine("LostMouseCapture");
            smallButton.Events.GotFocus += (sender, args) => System.Console.WriteLine("GotFocus");
            smallButton.Events.LostFocus += (sender, args) => System.Console.WriteLine("LostFocus");
            smallButton.Events.Clicked += (sender, args) => System.Console.WriteLine("Clicked");
            var largeButton =
                gui.Button(x: 200, y: 200, sizeX: 100, sizeY: 100)
               .Color(Color.Violet)
               .Text("Tester", 20, 20, gui.Fonts["segoe-10"])
               .TextColor(Color.Blue)
               .TextHorizontalAlignment(HorizontalAlignment.Center)
               .TextVerticalAlignment(VerticalAlignment.Center)
               .OnClick((sender, args) => System.Diagnostics.Trace.Write("i clicked a button"));

            gui.AddGuiHost("Main", smallButton, largeButton);
            gui.Show("Main");

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
