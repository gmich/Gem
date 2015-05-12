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
using Gem.Gui.Styles;

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

        public event EventHandler<SpriteBatch> DrawWith;

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
            this.HostTransition = () => TimedTransition.Default;
            screenManager = new ScreenManager(game,DrawTheRest);

            game.Components.Add(screenManager);
            game.Components.Add(new Input.InputManager(game));

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

        private void DrawTheRest(SpriteBatch batch)
        {
            var handler = DrawWith;
            if (handler != null)
            {
                handler(this, batch);
            }
        }

        #region Control Factory
        
        private Texture2D CreateDummyTexture()
        {
            var texture = new Texture2D(screenManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData(new[] { Color.White });

            return texture;
        }

        public IRenderStyle GetRenderStyle(Style style)
        {
            switch (style)
            {
                case Style.Transparent:
                    return new TransparentControlStyle();
                case Style.Border:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException("Style not found");
            }
        }

        public Button Button(int x, int y, int sizeX, int sizeY, Style style)
        {
            return controlFactory.CreateButton(new Region(new Vector2(x, y),
                                                           new Vector2(sizeX, sizeY)),
                                                           CreateDummyTexture(), GetRenderStyle(style));
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

        public Func<ITransition> HostTransition;

        #endregion

        #region Public Helper Methods
                
        public void AddGuiHost(string guiHostId, params AControl[] controls)
        {
            //foreach (var control in controls)
            //{
            //    //control.Align(new Region(Vector2.Zero, settings.Resolution));
            //    settings.OnResolutionChange((sender, args) =>
            //                                control.Align(new Region(Vector2.Zero, Settings.Resolution)));
            //}
            var guiHost = new GuiHost(controls.ToList(),
                                   settings.RenderTemplate,
                                   new AggregationContext(configuration.GetAggregators(aggregationTarget), controls),
                                   HostTransition());
            AddGuiHost(guiHostId, guiHost);

        }

        public IGuiHost this[string guiHostId]
        {
            get
            {
                if (hosts.ContainsKey(guiHostId))
                {
                    return hosts[guiHostId];
                }
                else
                {
                    throw new ArgumentException("Gui host was not found");
                }
            }
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

        public void Show(string guiHostId)
        {
            Contract.Requires(hosts.ContainsKey(guiHostId), "GuiHost was not found");
            screenManager.Enabled = true;

            screenManager.AddScreen(hosts[guiHostId]);
        }

        public bool IsShowing(string guiHostId)
        {
            return screenManager.IsShowing(hosts[guiHostId]);
        }

        public void Hide(string guiHostId)
        {
            Contract.Requires(hosts.ContainsKey(guiHostId), "GuiHost was not found");
            screenManager.Enabled = true;

            screenManager.RemoveScreen(hosts[guiHostId]);
        }

        public void Swap(string previousHost, string newHost)
        {
            Hide(previousHost);
            Show(newHost);
        }

        #endregion

    }
}