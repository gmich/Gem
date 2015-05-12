using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Gem.Gui.Rendering;

namespace Gem.Gui.ScreenSystem
{

    public class ScreenManager : DrawableGameComponent
    {

        private List<IGuiHost> hosts = new List<IGuiHost>();
        private SpriteBatch spriteBatch { get; set; }

        public ScreenManager(Game game)
            : base(game)
        { }


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
                        hosts[screenIndex].HandleInput();
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

        public override void Draw(GameTime gameTime)
        {
            foreach (var host in hosts)
            {
                switch (host.ScreenState)
                {
                    case ScreenState.Hidden:
                        continue;
                    case ScreenState.Active:
                        DrawHost(host);
                        break;
                    case ScreenState.TransitionOn:
                    case ScreenState.TransitionOff
                    :
                        var target = GetWindowRenderTarget();
                        GraphicsDevice.SetRenderTarget(target);
                        GraphicsDevice.Clear(Color.Transparent);
                        DrawHost(host);

                        GraphicsDevice.SetRenderTarget(null);
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                        host.Transition.Draw(target, host.ScreenState, spriteBatch);
                        spriteBatch.End();

                        break;
                    default:
                        continue;
                }
            }
            base.Draw(gameTime);
        }

        private void DrawHost(IGuiHost host)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            host.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void AddScreen(IGuiHost screen)
        {
            screen.EnterScreen();
            hosts.Add(screen);
        }

        public void RemoveScreen(IGuiHost screen)
        {
            screen.ExitScreen();
        }
    }
}