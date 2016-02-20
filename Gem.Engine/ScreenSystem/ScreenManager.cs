using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Gem.Engine.Configuration;
using Gem.Engine.Input;

using Gem.Engine.Console.Commands;
using Gem.Engine.GameLoop;

namespace Gem.Engine.ScreenSystem
{
    
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        private readonly List<RenderTarget2D> cachedTargets = new List<RenderTarget2D>();
        private readonly Dictionary<IScreenHost, RenderTarget2D> renderTargets = new Dictionary<IScreenHost, RenderTarget2D>();
        private readonly InputManager inputManager;

        private List<IScreenHost> hosts = new List<IScreenHost>();
        private SpriteBatch spriteBatch;
        private RenderTarget2D screen;
        public static Settings Settings { get; private set; }

        public static Terminal CommandTerminal { get; private set; }

        public ITimeline Time
        {
            get; set;
        } = Timeline.Default;

        #endregion

        #region Ctor

        public ScreenManager(Game game,Terminal terminal, InputManager inputManager, Settings settings, Action<SpriteBatch> drawWith)
            : base(game)
        {
            Settings = settings;
            this.DrawWith = drawWith;
            this.inputManager = inputManager;
            CommandTerminal = terminal;
        }

        #endregion

        #region Properties

        public Action<SpriteBatch> DrawWith { get; set; }

        public int ActiveHosts
        {
            get { return hosts.Count; }
        }

        public bool ShowsScreen
        {
            get { return this.ActiveHosts > 0; }
        }

        #endregion

        #region Private Helpers

        private RenderTarget2D GetWindowRenderTarget()
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

        private void AssignRenderTargetToDevice(RenderTarget2D target)
        {
            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(Color.Transparent);
        }

        private void DrawScreen(RenderTarget2D guiScreen)
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

        private void DrawHost(IScreenHost host)
        {
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            host.Draw(spriteBatch);
            //spriteBatch.End();
        }

        #endregion

        #region DrawableGameComponent Members

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screen = GetWindowRenderTarget();
            Settings.OnResolutionChange((sender, args) =>
            {
                cachedTargets.Clear();
                screen = GetWindowRenderTarget();
            });
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            (Timeline.Default as GameTimeline).GameTime = gameTime;

            for (int screenIndex = 0; screenIndex < hosts.Count; screenIndex++)
            {
                if (hosts[screenIndex].ScreenState == ScreenState.Exit)
                {
                    hosts.RemoveAt(screenIndex);
                }
                else
                {
                    hosts[screenIndex].Update(Time);
                    if (hosts[screenIndex].ScreenState == ScreenState.Active)
                    {
                        hosts[screenIndex].HandleInput(inputManager, Time);
                    }
                }
            }

            base.Update(gameTime);
        }
        
        public override void Draw(GameTime gameTime)
        {
            AssignRenderTargetToDevice(screen);

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

            DrawScreen(screen);
            DrawTransitions();

            base.Draw(gameTime);
        }

        #endregion

        #region Screen Transition Related

        public bool IsShowing(IScreenHost screen)
        {
            return hosts.Contains(screen);
        }

        public bool AddScreen(IScreenHost screen)
        {
            if (screen.ScreenState != ScreenState.Exit)
            {
                return false;
            }

            screen.EnterScreen();
            hosts.Add(screen);

            return true;
        }

        public void HideAll()
        {
            hosts.ForEach(host => host.ExitScreen());
        }

        public bool RemoveScreen(IScreenHost screen)
        {
            if (screen.ScreenState != ScreenState.Active)
            {
                return false;
            }
            screen.ExitScreen();

            return true;
        }

        #endregion
    }
}