using Microsoft.Xna.Framework;
using System;
using Gem.Gui.Input;
using Gem.Gui.Rendering;

namespace Gem.Gui.Configuration
{
    public sealed class Settings
    {

        #region Fields

        private readonly Game game;
        private static Vector2 resolution;
        private static Region viewRegion;
        private static Rectangle screenRectangle;
        private static Vector2 targetResolution;

        #endregion

        #region Ctor

        public Settings(Game game, Vector2 targetResolution)
        {
            this.game = game;
            game.Window.ClientSizeChanged += (sender, args) => SetResolution();
            Settings.targetResolution = targetResolution;
            ScaleCalculator = newResolution => newResolution / TargetResolution;            
            SetResolution();
        }

        #endregion

        #region Private Helper Methods

        private void SetResolution()
        {
            resolution = new Vector2(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            viewRegion = new Region(Vector2.Zero, resolution);
            screenRectangle = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            Scale = ScaleCalculator(resolution);
        }

        #endregion

        #region Event Related

        public void OnResolutionChange(EventHandler<EventArgs> args)
        {
            game.Window.ClientSizeChanged += args;
        }

        #endregion

        #region Properties

        //Calculates the scale according to the current resolution
        public Func<Vector2,Vector2> ScaleCalculator { get; set; }

        public InputManager.InputCapabilities InputCapabilities
        {
            get { return InputManager.GetInputCapabilities(true); }
        }

        public bool IsFullscreen { get { return game.GraphicsDevice.PresentationParameters.IsFullScreen; } }

        public bool IsMouseVisible { get { return game.IsMouseVisible; } }

        public Rectangle ViewPort { get { return game.Window.ClientBounds; } }

        public static Region ViewRegion { get { return viewRegion; } }

        public static Vector2 Resolution { get { return resolution; } }

        public static Rectangle ScreenRectangle { get { return screenRectangle; } }

        public static Vector2 TargetResolution { get { return targetResolution; } }

        public static Vector2 Scale { get; private set; }

        #endregion

    }
}
