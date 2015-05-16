using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Gem.Gui.Rendering;
using System;
using System.Linq;
using Gem.Gui.Configuration;

namespace Gem.Gui.ScreenSystem
{

    public class ScreenManager : DrawableGameComponent
    {

        private List<IGuiHost> hosts = new List<IGuiHost>();
        private SpriteBatch spriteBatch { get; set; }
        private readonly List<RenderTarget2D> cachedTargets = new List<RenderTarget2D>();
        private readonly Dictionary<IGuiHost, RenderTarget2D> renderTargets = new Dictionary<IGuiHost, RenderTarget2D>();
        public Action<SpriteBatch> DrawWith;
        private RenderTarget2D guiScreen;

        public ScreenManager(Game game, Settings settings, Action<SpriteBatch> drawWith)
            : base(game)
        {
            settings.OnResolutionChange((sender, args) =>
            {
                 this.cachedTargets.Clear();
                 this.guiScreen = GetWindowRenderTarget();
            });
            this.guiScreen = GetWindowRenderTarget();
            this.DrawWith = drawWith;
        }


        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            for (int screenIndex = 0; screenIndex < hosts.Count; screenIndex++)
            {
                if (hosts[screenIndex].ScreenState == ScreenState.Exit)
                {
                    hosts.RemoveAt(screenIndex);
                }
                else
                {
                    hosts[screenIndex].Update(gameTime);
                    if (hosts[screenIndex].ScreenState == ScreenState.Active)
                    {
                        hosts[screenIndex].HandleInput(gameTime);
                    }
                }
            }

            base.Update(gameTime);
        }

        public RenderTarget2D GetWindowRenderTarget()
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            return new RenderTarget2D(GraphicsDevice,
                                    pp.BackBufferWidth,
                                    pp.BackBufferHeight,
                                    false,
                                    SurfaceFormat.Color,
                                    pp.DepthStencilFormat,
                                    pp.MultiSampleCount,
                                    RenderTargetUsage.DiscardContents);
        }

        public void AssignRenderTargetToDevice(RenderTarget2D target)
        {
            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(Color.Transparent);
        }

        public override void Draw(GameTime gameTime)
        {
            AssignRenderTargetToDevice(guiScreen);

            DrawWith(spriteBatch);

            renderTargets.Clear();

            foreach (var host in hosts.Where(host => host.ScreenState == ScreenState.Active))
            {
                DrawHost(host);
            }

            var hostsWithTransition = hosts.Where(host => host.ScreenState == ScreenState.TransitionOn
                                                       || host.ScreenState == ScreenState.TransitionOff);

            int currenthost = 1;
            foreach (var host in hostsWithTransition)
            {
                //if there aren't enough rendertargets, create a new one
                if (cachedTargets.Count < currenthost)
                {
                    cachedTargets.Add(GetWindowRenderTarget());
                }

                RenderTarget2D target = cachedTargets[currenthost - 1];
                AssignRenderTargetToDevice(target);
                DrawHost(host);
                renderTargets.Add(host, target);
                currenthost++;
            }

            GraphicsDevice.SetRenderTarget(null);

            DrawGui(guiScreen);
            DrawTransitions();

            base.Draw(gameTime);
        }

        public void DrawGui(RenderTarget2D guiScreen)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Draw(guiScreen, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private void DrawTransitions()
        {
            if (renderTargets.Count == 0) return;

            foreach (var target in renderTargets)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                target.Key.Transition.Draw(target.Value, target.Key.ScreenState, spriteBatch);
                spriteBatch.End();
            }
        }

        private void DrawHost(IGuiHost host)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            host.Draw(spriteBatch);
            spriteBatch.End();

        }

        public bool IsShowing(IGuiHost screen)
        {
            return hosts.Contains(screen);
        }

        public bool AddScreen(IGuiHost screen)
        {
            if (screen.ScreenState != ScreenState.Exit)
            {
                return false;
            }

            screen.EnterScreen();
            hosts.Add(screen);

            return true;
        }

        public bool RemoveScreen(IGuiHost screen)
        {
            if (screen.ScreenState != ScreenState.Active)
            {
                return false;
            }
            screen.ExitScreen();

            return true;
        }
    }
}