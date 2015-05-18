#region Usings

using Gem.Gui.Aggregation;
using Gem.Gui.Configuration;
using Gem.Gui.Containers;
using Gem.Gui.Controls;
using Gem.Gui.Factories;
using Gem.Gui.Rendering;
using Gem.Gui.ScreenSystem;
using Gem.Infrastructure.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Gem.Gui.Layout;
using Gem.Gui.Alignment;
using Gem.Gui.Styles;

#endregion

namespace Gem.Gui
{
    public sealed class GemGui : IDisposable
    {

        #region Fields

        private readonly IConfigurationResolver configuration;
        private readonly IControlFactory controlFactory;
        private readonly AggregationTarget aggregationTarget;
        private readonly ITextureFactory textureFactory;

        private readonly Dictionary<string, IGuiHost> hosts = new Dictionary<string, IGuiHost>();
        private readonly AssetContainer<SpriteFont> _fontContainer;
        private readonly AssetContainer<Texture2D> _textureContainer;

        private readonly Settings settings;
        private readonly ScreenManager screenManager;

        public event EventHandler<SpriteBatch> DrawWith;

        #endregion

        #region Construct / Dispose

        public GemGui(Game game,
                      int targetResolutionWidth,
                      int targetResolutionHeight,
                      AggregationTarget aggregationTarget = AggregationTarget.All,
                      TargetPlatform controlTarget = TargetPlatform.Windows,
                      IConfigurationResolver configuration = null)
        {
            this.configuration = configuration ?? new DefaultConfiguration();
            this.aggregationTarget = aggregationTarget;
            this.settings = new Settings(game, new Vector2(targetResolutionWidth, targetResolutionHeight));
            this.controlFactory = this.configuration.GetControlFactory(controlTarget);
            this.HostTransition = () => TimedTransition.Default;
            this.screenManager = new ScreenManager(game, settings, DrawTheRest);
            this.textureFactory = new TextureFactory(game.GraphicsDevice);

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
                hosts.Clear();
                isDisposed = true;
            }
        }

        #endregion

        #region Public Properties

        public Settings Settings { get { return settings; } }

        public AssetContainer<SpriteFont> Fonts { get { return _fontContainer; } }

        public AssetContainer<Texture2D> Textures { get { return _textureContainer; } }

        public Func<ITransition> HostTransition { get; set; }

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

        #endregion

        #region Private Helper Methods

        private void DrawTheRest(SpriteBatch batch)
        {
            var handler = DrawWith;
            if (handler != null)
            {
                handler(this, batch);
            }
        }

        private Texture2D GetDefaultTexture(int width, int height, IPattern pattern)
        {
            return textureFactory.GetTexture(
                new TextureCreationRequest(width, height,
                                           Color.White,
                                           pattern));
        }

        #endregion

        #region Control Factory

        public ARenderStyle GetRenderStyle(Style style)
        {
            switch (style)
            {
                case Style.Transparent:
                    return new TransparentControlStyle();
                case Style.NoStyle:
                    return new NoStyle();
                case Style.Border:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentException("Style not found");
            }
        }

        public Button Button(int x, int y,
                             int sizeX, int sizeY,
                             Style style,
                             IPattern pattern = null)
        {
            return controlFactory.CreateButton(new Region(new Vector2(x, y),
                                                           new Vector2(sizeX, sizeY)),
                                                           GetDefaultTexture(sizeX, sizeY, pattern ?? Pattern.SolidColor),
                                                           GetRenderStyle(style));
        }

        public ListView ListView(int x, int y,
                                 int sizeX, int sizeY,
                                 Orientation orientation,
                                 IHorizontalAlignable horizontalAlignment,
                                 IVerticalAlignable verticalAlignment,
                                 IAlignmentTransition alignmentTransition,
                                 IPattern pattern = null,
                                 params AControl[] controls)
        {
            return controlFactory.CreateListView(GetDefaultTexture(sizeX, sizeY, pattern ?? Pattern.SolidColor),
                                                 new Region(new Vector2(x, y),
                                                            new Vector2(sizeX, sizeY)),
                                                 orientation,
                                                 new AlignmentContext(horizontalAlignment, verticalAlignment, alignmentTransition),
                                                 controls.ToList().AsReadOnly());
        }

        public Label Label(int x, int y,
                           int sizeX, int sizeY,
                           string text,
                           SpriteFont font,
                           Color textcolor,
                           IHorizontalAlignable horizontalAlignment = null,
                           IVerticalAlignable verticalAlignment = null,
                           IAlignmentTransition alignmentTransition = null,
                           IPattern pattern = null,
                           TextAppenderHelper appender = null)
        {
            return controlFactory.CreateLabel(text,
                                              font,
                                              textcolor,
                                              GetDefaultTexture(sizeX, sizeY, pattern ?? Pattern.SolidColor),
                                              new Region(new Vector2(x, y),
                                                         new Vector2(sizeX, sizeY)),
                                              horizontalAlignment ?? HorizontalAlignment.Left,
                                              verticalAlignment ?? VerticalAlignment.Center,
                                              alignmentTransition ?? AlignmentTransition.Fixed);
        }

        public TextField TextField(int x, int y,
                         int sizeX, int sizeY,
                         Color textColor,
                         SpriteFont font,
                         Style style,
                         string hint = null,
                         IHorizontalAlignable horizontalAlignment = null,
                         IVerticalAlignable verticalAlignment = null,
                         IAlignmentTransition alignmentTransition = null,
                         IPattern pattern = null,
                         TextAppenderHelper appender = null)
        {
            TextField textField = null;

            return textField =
                   controlFactory.CreateTextBox(appender ?? TextAppenderHelper.Default,
                                                font,
                                                GetDefaultTexture(sizeX, sizeY, pattern ?? Pattern.SolidColor),
                                                new Region(new Vector2(x, y),
                                                            new Vector2(sizeX, sizeY)),
                                                textColor,
                                                GetRenderStyle(style),
                                                hint ?? string.Empty,
                                                horizontalAlignment ?? HorizontalAlignment.Left,
                                                verticalAlignment ?? VerticalAlignment.Center,
                                                alignmentTransition ?? AlignmentTransition.Fixed);
        }

        //TODO: add the rest

        #endregion

        #region Public Methods

        public void AddGuiHost(string guiHostId, params AControl[] controls)
        {
            foreach (var control in controls)
            {
                settings.OnResolutionChange((sender, args) => control.Scale(Settings.Scale));
            }
            var entries = controls.Where(control => control.HasAttribute<LayoutAttribute>());
            var controlsEnumerable = controls.AsEnumerable();

            foreach (var entry in entries)
            {
                controlsEnumerable = controlsEnumerable.Concat(entry.Entries());
            }

            var guiHost = new GuiHost(controls.ToList(),
                                      new AggregationContext(configuration.GetAggregators(aggregationTarget).ToList(), controlsEnumerable),
                                      HostTransition());
            AddGuiHost(guiHostId, guiHost);
        }

        public void AddGuiHost(string guiHostId, IGuiHost guiHost)
        {
            Contract.Requires(!hosts.ContainsKey(guiHostId), "A GuiHost with the same id is already registered");
            this.hosts.Add(guiHostId, guiHost);
        }

        public void Disable()
        {
            screenManager.Visible = false;
            screenManager.Enabled = false;
        }

        public void Enable()
        {
            screenManager.Visible = true;
            screenManager.Enabled = true;
        }

        public bool IsEnabled
        {
            get { return screenManager.Enabled; }
        }

        public bool ShowsAnything
        {
            get { return screenManager.ActiveHosts > 0; }
        }

        public bool Show(string guiHostId)
        {
            Contract.Requires(hosts.ContainsKey(guiHostId), "GuiHost was not found");

            return screenManager.AddScreen(hosts[guiHostId]);
        }

        public bool IsShowing(string guiHostId)
        {
            return screenManager.IsShowing(hosts[guiHostId]);
        }

        public bool Hide(string guiHostId)
        {
            Contract.Requires(hosts.ContainsKey(guiHostId), "GuiHost was not found");

            return screenManager.RemoveScreen(hosts[guiHostId]);
        }

        public bool Swap(string previousHost, string newHost)
        {
            return Hide(previousHost) && Show(newHost);
        }

        #endregion

    }
}