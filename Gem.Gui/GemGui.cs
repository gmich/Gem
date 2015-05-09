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

        private readonly Dictionary<string, GuiHost> hosts = new Dictionary<string, GuiHost>();
        private static ScreenManager screenManager;
        private static AssetContainer<Texture2D> textureContainer;
        private static AssetContainer<SpriteFont> fontContainer;

        #endregion

        #region Construct / Dispose

        public GemGui(Game game)
        {
            screenManager = new ScreenManager(game);
            textureContainer = new AssetContainer<Texture2D>(game.Content);
            fontContainer = new AssetContainer<SpriteFont>(game.Content);

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

        #region Static Control Factory

        private static IControlFactory controlFactory = null; ////TODO: provide from ioc container

        public static Button Button(int x, int y, int sizeX, int sizeY, Func<Region, Vector2> originCalculator = null)
        {
            return controlFactory.CreateButton(new Region(new Vector2(x, y),
                                                          new Vector2(sizeX, sizeY)),
                                                          originCalculator,
                                                          new Texture2D(screenManager.GraphicsDevice, 1, 1));
        }

        public static ImageButton ImageButton()
        {
            throw new NotImplementedException();
        }

        //TODO: add the rest

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