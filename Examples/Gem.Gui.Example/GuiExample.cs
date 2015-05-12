using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Gem.Gui.Fluent;
using Gem.Gui.Alignment;
using Gem.Gui.Styles;

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

            var firstButton =
               gui.Button(20, 20, 100, 100, style: Style.Transparent)
                  .Color(Color.White)
                  .Text("First", 0, 0, gui.Fonts["segoe-10"])
                  .TextColor(Color.Black)
                  .TextHorizontalAlignment(HorizontalAlignment.Center)
                  .TextVerticalAlignment(VerticalAlignment.Center)
                  .OnClick((sender, args) => gui.Swap("First", "Second"));

            firstButton.Events.GotMouseCapture += (sender, args) => firstButton.Region.Size = new Vector2(150,150);
            firstButton.Events.LostMouseCapture += (sender, args) => firstButton.Region.Size = new Vector2(100,100);
            firstButton.Events.LostFocus += (sender, args) => System.Console.WriteLine("LostFocus");
            firstButton.Events.Clicked += (sender, args) => System.Console.WriteLine("Clicked");

            gui.AddGuiHost("First", firstButton);

            var secondButton =
                gui.Button(x: 200, y: 200, sizeX: 100, sizeY: 100, style: Style.Transparent)
                   .Color(Color.Violet)
                   .Text("Second", 20, 20, gui.Fonts["segoe-10"])
                   .TextColor(Color.Blue)
                   .TextHorizontalAlignment(HorizontalAlignment.Center)
                   .TextVerticalAlignment(VerticalAlignment.Center)
                   .OnClick((sender, args) =>
                   {
                       if (gui.IsShowing("Third"))
                           gui.Hide("Third");
                       else
                           gui.Show("Third");
                   });

            gui.AddGuiHost("Second", secondButton);

            var thirdButton =
                gui.Button(x: 300, y: 300, sizeX: 100, sizeY: 100, style: Style.Transparent)
               .Color(Color.Aqua)
               .Text("Third", 20, 20, gui.Fonts["segoe-10"])
               .TextColor(Color.Blue)
               .TextHorizontalAlignment(HorizontalAlignment.Center)
               .TextVerticalAlignment(VerticalAlignment.Center)
               .OnClick((sender, args) =>
                   {
                       gui.Hide("Third");
                       gui.Swap("Second", "First");
                   });
            gui.AddGuiHost("Third", thirdButton);

            gui.Show("First");

            gui.DrawWith += (sender, batch) => RenderBackground(batch);
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
            base.Draw(gameTime);
        }

        public void RenderBackground(SpriteBatch batch)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            batch.DrawString(gui.Fonts["segoe-10"], "if you see me, then the background is preserved", new Vector2(20, 20), Color.Wheat);
            batch.End();
        }
    }
}
