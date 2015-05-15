using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Gem.Gui.Fluent;
using Gem.Gui.Alignment;
using Gem.Gui.Styles;
using Gem.Gui.Layout;
using Gem.Gui.Controls;
using Gem.Gui.ScreenSystem;
using System;

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
                  .Text(gui.Fonts["segoe-10"], "First")
                  .TextColor(Color.Black)
                  .TextHorizontalAlignment(HorizontalAlignment.Center)
                  .TextVerticalAlignment(VerticalAlignment.Center)
                  .OnClick((sender, args) => gui.Swap("First", "Second"));

            var rand =
               gui.Button(30, 30, 100, 100, style: Style.Transparent)
                  .Color(Color.White)
                  .Text(gui.Fonts["segoe-10"], "rand", 10, 10, true)
                  .TextColor(Color.Black)
                  .OnClick((sender, args) => gui.Swap("Second", "First"));


            rand.ScreenAlignment.HorizontalAlignment = HorizontalAlignment.Center;
            firstButton.Events.GotMouseCapture += (sender, args) => System.Console.WriteLine("GotMouseCapture");
            firstButton.Events.LostMouseCapture += (sender, args) => System.Console.WriteLine("LostMouseCapture");
            firstButton.Events.LostFocus += (sender, args) => System.Console.WriteLine("LostFocus");
            firstButton.Events.GotFocus += (sender, args) => System.Console.WriteLine("GotFocus");
            firstButton.Events.Clicked += (sender, args) => System.Console.WriteLine("Clicked");

            var textBox = gui.TextBox(x: 300, y: 100,
                                      sizeX: 200, sizeY: 70,
                                      textColor: Color.Black,
                                      font: gui.Fonts["segoe-10"],
                                      style: Style.Transparent);

            gui.AddGuiHost("First", firstButton, textBox);

            var secondButton =
                gui.Button(x: 200, y: 200, sizeX: 100, sizeY: 100, style: Style.Transparent)
                   .Color(Color.Violet)
                   .Text(gui.Fonts["segoe-10"], "1231")
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


            gui.AddGuiHost("Second", secondButton, rand);


            gui["First"].Transition = new TimedTransition(TimeSpan.FromSeconds(0.3),
                                         (state, progress, target, batch) =>
                                           batch.Draw(target, Vector2.Zero + (progress - 1.0f) * new Vector2(500, 0), Color.White * progress));
 
            gui["Second"].Transition = new TimedTransition(TimeSpan.FromSeconds(0.3),
                                         (state, progress, target, batch) =>
                                          batch.Draw(target, Vector2.Zero, Color.White * progress));
            var thirdButton =
                gui.Button(x: 300, y: 300, sizeX: 100, sizeY: 100, style: Style.Transparent)
               .Color(Color.Aqua)
               .Text(gui.Fonts["segoe-10"], "12342331")
               .TextColor(Color.Blue)
               .TextHorizontalAlignment(HorizontalAlignment.Center)
               .TextVerticalAlignment(VerticalAlignment.Center)
               .OnClick((sender, args) =>
                   {
                       gui.Hide("Third");
                       gui.Hide("Second");
                       gui.Show("LayoutHost");
                   });
            gui.AddGuiHost("Third", thirdButton);

            AddListView();
            gui.DrawWith += (sender, batch) => RenderBackground(batch);

            gui.Show("First");

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void AddListView()
        {
            AControl listView = null;
            var one =
                gui.Button(20, 20, 70, 70, style: Style.Transparent)
                   .Color(Color.White)
                   .Text(gui.Fonts["segoe-10"], "1")
                   .TextHorizontalAlignment(HorizontalAlignment.Center)
                   .TextVerticalAlignment(VerticalAlignment.Center)
                   .TextColor(Color.Black)
                   .OnClick((sender, args) => gui.Swap("LayoutHost", "First"));

            one.Padding.Bottom = 10;
            one.Padding.Top = 10;

            var two =
                gui.Button(100, 100, 50, 50, style: Style.Transparent)
                   .Color(Color.White)
                   .Text(gui.Fonts["segoe-10"], "2", 10, 10, true)
                   .TextColor(Color.Black)
                   .OnClick((sender, args) => listView.Region.Size = new Vector2(200, 300));

            //two.Padding.Top =20;
            two.Padding.Bottom = 40;
            var three =
                gui.Button(200, 100, 50, 50, style: Style.Transparent)
                   .Color(Color.White)
                   .Text(gui.Fonts["segoe-10"], "3", 5, 5, true)
                   .TextColor(Color.Black)
                   .OnClick((sender, args) =>
                       listView.Region.Size = new Vector2(300, 400));

            listView =
                gui.ListView(x: 10, y: 10,
                            sizeX: 200, sizeY: 400,
                            orientation: Layout.Orientation.Portrait,
                            horizontalAlignment: HorizontalAlignment.Center,
                            verticalAlignment: VerticalAlignment.Bottom,
                            alignmentTransition: AlignmentTransition.Fixed,
                            controls: new[] { one, two, three })
                    .ScreenAlignment(HorizontalAlignment.Center, VerticalAlignment.Center)
                    .Color(Color.Red);

            gui.AddGuiHost("LayoutHost", listView);
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

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
