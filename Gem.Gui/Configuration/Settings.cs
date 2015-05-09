using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gem.Gui.Input;
using Gem.Gui.Controls;
using Gem.Gui.Aggregation;

namespace Gem.Gui.Configuration
{
    public sealed class Settings
    {

        #region Fields

        private readonly Game game;
        private Vector2 resolution;

        #endregion

        #region Ctor

        public Settings(Game game)
        {
            this.game = game;
            SetResolution();
            game.Window.ClientSizeChanged += (sender, args) => SetResolution();
        }

        #endregion

        #region Private Helper Methods

        private void SetResolution()
        {
            resolution = new Vector2(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
        }

        #endregion

        #region Event Related

        public void OnResolutionChange(EventHandler<EventArgs> args)
        {
            game.Window.ClientSizeChanged += args;
        }

        #endregion

        #region Properties

        public InputManager.InputCapabilities InputCapabilities
        {
            get { return InputManager.GetInputCapabilities(true); }
        }

        public bool IsFullscreen { get { return game.GraphicsDevice.PresentationParameters.IsFullScreen; } }

        public bool IsMouseVisible { get { return game.IsMouseVisible; } }

        public Rectangle ViewWindow { get { return game.Window.ClientBounds; } }

        public Vector2 Resolution { get { return resolution; } }


        #endregion

    }
}
