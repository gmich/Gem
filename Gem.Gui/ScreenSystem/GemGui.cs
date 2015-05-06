using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Gem.Gui.ScreenSystem
{

    /// <summary>
    /// Maintains a stack of screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {

        private List<IGuiHost> screens = new List<IGuiHost>();
        private List<IGuiHost> screensToUpdate = new List<IGuiHost>();
        private List<RenderTarget2D> transitions = new List<RenderTarget2D>();


        public ScreenManager(Game game)
            : base(game)
        {
            Content = game.Content;
            Content.RootDirectory = "Content";
        }

        public IGuiHost ActiveHost
        {
            get
            {
                if (screens.Count > 1)
                {
                    if (screens[1].ScreenState == ScreenState.Active)
                        return screens[1];
                    else
                        return screens[0];
                }
                return null;
            }
        }

        public SpriteBatch SpriteBatch { get; private set; }

        public ContentManager Content { get; private set; }

        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        public override void Initialize()
        {

        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public void Reload()
        {
            transitions = new List<RenderTarget2D>();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
        

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            //input.Update(gameTime);

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            screensToUpdate.Clear();

            foreach (var screen in screens)
            {
                screensToUpdate.Add(screen);
            }

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (screensToUpdate.Count > 0)
            {
                var screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput();
                        otherScreenHasFocus = true;
                    }
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            int transitionCount = 0;
            foreach (var screen in screens)
            {
                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.TransitionOff)
                {
                    ++transitionCount;
                    if (transitions.Count < transitionCount)
                    {
                        PresentationParameters _pp = GraphicsDevice.PresentationParameters;
                        transitions.Add(new RenderTarget2D(GraphicsDevice, _pp.BackBufferWidth, _pp.BackBufferHeight, false, SurfaceFormat.Color, _pp.DepthStencilFormat, _pp.MultiSampleCount, RenderTargetUsage.DiscardContents));
                    }
                    GraphicsDevice.SetRenderTarget(transitions[transitionCount - 1]);
                    GraphicsDevice.Clear(Color.Transparent);
                    screen.Draw();
                    GraphicsDevice.SetRenderTarget(null);
                }
            }

            GraphicsDevice.Clear(Color.Black);

            transitionCount = 0;
            foreach (var screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.TransitionOff)
                {
                    SpriteBatch.Begin(0, BlendState.AlphaBlend);
                    SpriteBatch.Draw(transitions[transitionCount], Vector2.Zero, Color.White * screen.TransitionAlpha);
                    SpriteBatch.End();

                    ++transitionCount;
                }
                else
                    screen.Draw();
            }
        }

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(IGuiHost screen)
        {
            foreach (var scr in screens)
            {
                if (scr.GetType() == screen.GetType())
                    return;
            }

            screens.Add(screen);
        }

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScreen(IGuiHost screen)
        {
            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }
    }
}