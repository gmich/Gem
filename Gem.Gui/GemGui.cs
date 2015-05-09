using Gem.Gui.Aggregation;
using Gem.Gui.Configuration;
using Gem.Gui.Containers;
using Gem.Gui.Controls;
using Gem.Gui.Factories;
using Gem.Gui.Rendering;
using Gem.Gui.ScreenSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Gem.Gui
{
    public sealed class GemGui : IDisposable
    {

        #region Fields

        private readonly IConfigurationResolver configuration;
        private readonly IControlDrawable controlDrawable = Fluent.RenderControlBy.Frame;
        private readonly ITextDrawable textDrawable = Fluent.RenderTextBy.Position;
        private readonly IControlFactory controlFactory;

        private readonly AssetContainer<SpriteFont> _fontContainer;
        private readonly AssetContainer<Texture2D> _textureContainer;

        private readonly Settings settings;
        private readonly Dictionary<string, GuiHost> hosts = new Dictionary<string, GuiHost>();
        private ScreenManager screenManager;

        #endregion

        #region Construct / Dispose

        public GemGui(Game game,
                      AggregationTarget aggregationTarget = AggregationTarget.All,
                      ControlTarget controlTarget = ControlTarget.Irrelevant,
                      IConfigurationResolver configuration = null)
        {
            this.configuration = configuration ?? new DefaultConfiguration();
            this.settings = new Settings(game);
            this.controlFactory = this.configuration.GetControlFactory(controlTarget);
            screenManager = new ScreenManager(game);
            _fontContainer = new AssetContainer<SpriteFont>(game.Content);
            _textureContainer = new AssetContainer<Texture2D>(game.Content);
            game.Components.Add(screenManager);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool isDisposed = false;
        private void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                screenManager.Dispose();
                isDisposed = true;
            }
        }

        #endregion

        #region Control Factory


        public Button Button(int x, int y, int sizeX, int sizeY, Func<Region, Vector2> originCalculator = null)
        {
            return controlFactory.CreateButton(new Region(new Vector2(x, y),
                                                           new Vector2(sizeX, sizeY)),
                                                           originCalculator,
                                                           new Texture2D(screenManager.GraphicsDevice, 1, 1));
        }

        //TODO: add the rest

        #endregion

        #region Properties

        public AssetContainer<SpriteFont> Fonts { get { return _fontContainer; } }

        public AssetContainer<Texture2D> Textures { get { return _textureContainer; } }

        #endregion

        #region Public Helper Methods

        public void AddGuiHost(string guiHostId, GuiHost host)
        {
            Contract.Requires(!hosts.ContainsKey(guiHostId), "A GuiHost with the same id is already registered");
            this.hosts.Add(guiHostId, host);
        }

        public void Disable()
        {
            screenManager.Enabled = false;
        }

        public void SwitchTo(string guiHostId)
        {
            Contract.Requires(hosts.ContainsKey(guiHostId), "GuiHost was not found");

            screenManager.Enabled = true;
            screenManager.AddScreen(hosts[guiHostId]);
        }

        #endregion

    }
}