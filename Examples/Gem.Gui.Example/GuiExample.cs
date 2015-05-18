using Gem.Gui.Aggregation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Example
{
    public class GuiExample : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private GemGui gui;
        private SpriteBatch batch;

        public string PlayerName { get; set; }

        public GuiExample()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.PlayerName = string.Empty;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            batch = new SpriteBatch(GraphicsDevice);
            int targetResolutionX = 800;
            int targetResolutionY = 480;

            gui = new GemGui(this, 
                             targetResolutionX, targetResolutionY,
                             AggregationTarget.Mouse | AggregationTarget.Keyboard | AggregationTarget.GamePad);

            gui.Settings.ScaleCalculator = resolution =>
                resolution / new Vector2(targetResolutionX, targetResolutionY);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            gui.Fonts.Add("segoe-10", @"Fonts/segoe-10");
            gui.Textures.Add("frame", @"Common/frame");

            new MainMenuScreen(gui, this);
            new NewGameScreen(gui, this);
            new SettingsScreen(gui, this);           
            gui.DrawWith += (sender, batch) => RenderBackground(batch);

            gui.Show(GuiScreen.MainMenu);
        }

        protected override void Update(GameTime gameTime)
        {
            if(Input.InputManager.Keyboard.IsKeyClicked(Microsoft.Xna.Framework.Input.Keys.Escape)
                && !gui.IsEnabled)
            {  
                gui.Enable();
                gui.Show(GuiScreen.MainMenu);
            }

            base.Update(gameTime);
        }
        public void RenderBackground(SpriteBatch batch)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            batch.DrawString(gui.Fonts["segoe-10"], "You are: " + PlayerName, Vector2.Zero, Color.White);
            batch.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (gui.IsEnabled) return;
            RenderBackground(batch);
        }
    }
}
