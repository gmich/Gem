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
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Gem.Gui.Layout;
using Gem.Gui.Alignment;

namespace Gem.Gui
{
    public sealed class GemGui : IDisposable
    {

        #region Fields

        private readonly IConfigurationResolver configuration;
        private readonly IControlFactory controlFactory;
        private readonly AggregationTarget aggregationTarget;

        private readonly AssetContainer<SpriteFont> _fontContainer;
        private readonly AssetContainer<Texture2D> _textureContainer;

        private readonly Settings settings;
        private readonly Dictionary<string, IGuiHost> hosts = new Dictionary<string, IGuiHost>();
        private ScreenManager screenManager;

        #endregion

        #region Construct / Dispose

        public GemGui(Game game,
                      AggregationTarget aggregationTarget = AggregationTarget.All,
                      ControlTarget controlTarget = ControlTarget.Irrelevant,
                      IConfigurationResolver configuration = null)
        {
            this.configuration = configuration ?? new DefaultConfiguration();
            this.aggregationTarget = aggregationTarget;
            this.settings = new Settings(game);
            this.controlFactory = this.configuration.GetControlFactory(controlTarget);
            screenManager = new ScreenManager(game);
            game.Components.Add(screenManager);

            _fontContainer = new AssetContainer<SpriteFont>(game.Content);
            _textureContainer = new AssetContainer<Texture2D>(game.Content);
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

        private Texture2D CreateDummyTexture()
        {
            var texture = new Texture2D(screenManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData(new[] { Color.White });

            return texture;
        }

        public Button Button(int x, int y, int sizeX, int sizeY, Func<Region, Vector2> originCalculator = null)
        {
            return controlFactory.CreateButton(new Region(new Vector2(x, y),
                                                           new Vector2(sizeX, sizeY),
                                                           originCalculator),
                                                           CreateDummyTexture());
        }

        public ListView ListView(int x, int y, int sizeX, int sizeY,
                                 Orientation orientation,
                                 AlignmentContext alignmentContext = null,
                                 params AControl[] controls)
        {
            return controlFactory.CreateListView(CreateDummyTexture(),
                                                 new Region(new Vector2(x, y),
                                                            new Vector2(sizeX, sizeY)),
                                                 orientation,
                                                 alignmentContext ?? AlignmentContext.Default,
                                                 controls.ToList().AsReadOnly());
        }

        //TODO: add the rest

        #endregion

        #region Properties

        public AssetContainer<SpriteFont> Fonts { get { return _fontContainer; } }

        public AssetContainer<Texture2D> Textures { get { return _textureContainer; } }

        #endregion

        #region Public Helper Methods

        public void AddGuiHost(string guiHostId, params AControl[] controls)
        {
            foreach (var control in controls)
            {
                control.Align(new Region(Vector2.Zero, settings.Resolution));
                settings.OnResolutionChange((sender, args) =>
                                            control.Align(new Region(Vector2.Zero, settings.Resolution)));
            }
            AddGuiHost(guiHostId,
                       new GuiHost(controls.ToList(),
                                   new AggregationContext(configuration.GetAggregators(aggregationTarget), controls)));
        }

        public void AddGuiHost(string guiHostId, IGuiHost guiHost)
        {
            Contract.Requires(!hosts.ContainsKey(guiHostId), "A GuiHost with the same id is already registered");
            this.hosts.Add(guiHostId, guiHost);
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