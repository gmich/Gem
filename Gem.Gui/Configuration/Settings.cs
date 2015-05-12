﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gem.Gui.Input;
using Gem.Gui.Controls;
using Gem.Gui.Aggregation;
using Gem.Gui.Rendering;
using Gem.Gui.Fluent;

namespace Gem.Gui.Configuration
{
    public sealed class Settings
    {

        #region Fields

        private readonly Game game;
        private static Vector2 resolution;
        private static Region viewRegion;

        #endregion

        #region Ctor

        public Settings(Game game)
        {
            this.game = game;
            this.RenderTemplate = new Rendering.RenderTemplate(RenderControlBy.Frame, RenderTextBy.Position);
            SetResolution();
            game.Window.ClientSizeChanged += (sender, args) => SetResolution();
        }

        #endregion

        #region Private Helper Methods

        private void SetResolution()
        {
            resolution = new Vector2(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            viewRegion = new Region(Vector2.Zero, resolution);
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

        public Rectangle ViewPort { get { return game.Window.ClientBounds; } }

        public static Region ViewRegion { get { return viewRegion; } }

        public static Vector2 Resolution { get { return resolution; } }

        public RenderTemplate RenderTemplate { get; set; }

        #endregion

    }
}
