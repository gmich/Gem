using Gem.Gui.Controls;
using Gem.Gui.Factories;
using Gem.Gui.ScreenSystem;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Gem.Gui
{
    public class GemGui : IDisposable
    {

        private readonly Dictionary<string, GuiHost> hosts = new Dictionary<string, GuiHost>();
        private readonly ScreenManager screenManager;

        #region Construct / Dispose

        public GemGui(Game game)
        {
            screenManager = new ScreenManager(game);
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

        private static IControlFactory controlFactory = null; ////TODO: provide from ioc container

        public static Button Button
        {
            get { return controlFactory.CreateButton(); }
        }

        public static ImageButton ImageButton
        {
            get { return controlFactory.CreateImageButton(); }
        }

        #endregion


        public void AddGuiHost(string guiHostId, GuiHost host)
        {
            if (hosts.ContainsKey(guiHostId))
            {
                throw new ArgumentException("A GuiHost with the same id is already registered");
            }

            this.hosts.Add(guiHostId, host);

        }

        public void Disable()
        {
            screenManager.Enabled = false;
        }

        public void SwicthTo(string guiHostId)
        {
            if (!hosts.ContainsKey(guiHostId))
            {
                throw new ArgumentException("GuiHost was not found");
            }
            screenManager.Enabled = true;
            screenManager.AddScreen(hosts[guiHostId]);
        }

    }
}