using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.ScreenSystem
{
    /// <summary>
    /// Enum describes the screen transition state.
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    public abstract class AnimatedGuiScreen : IGuiHost
    {
        private bool otherScreenHasFocus;

        public AnimatedGuiScreen()
        {
            TransitionPosition = 1;
            TransitionOffTime = TimeSpan.Zero;
            TransitionOnTime = TimeSpan.Zero;
        }

        private bool isPopup;
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        public TimeSpan TransitionOnTime { get; protected set; }

        public TimeSpan TransitionOffTime { get; protected set; }

        public float TransitionPosition { get; protected set; }

        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }

        public ScreenState ScreenState { get; protected set; }

        public bool IsExiting { get; protected internal set; }

        /// <summary>
        /// Checks whether this screen is active and can respond to user input.
        /// </summary>
        public bool IsActive { get { return !otherScreenHasFocus && (ScreenState == ScreenState.TransitionOn || ScreenState == ScreenState.Active); } }


        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager { get; internal set; }

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (IsExiting)
            {
                ScreenState = ScreenState.TransitionOff;

                // When the transition finishes, remove the screen.
                if (!UpdateTransition(gameTime, TransitionOffTime, 1))
                    ScreenManager.RemoveScreen(this);
            }
            else if (coveredByOtherScreen)
            {
                ScreenState = UpdateTransition(gameTime, TransitionOffTime, 1) ? ScreenState.TransitionOff : ScreenState.Hidden;
            }
            else
            {
                ScreenState = UpdateTransition(gameTime, TransitionOnTime, -1) ? ScreenState.TransitionOn : ScreenState.Active;
            }
        }


        private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {

            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1f;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);


            TransitionPosition += transitionDelta * direction;

            if (((direction < 0) && (TransitionPosition <= 0)) || ((direction > 0) && (TransitionPosition >= 1)))
            {
                TransitionPosition = MathHelper.Clamp(TransitionPosition, 0, 1);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public virtual void HandleInput()
        {
        }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public virtual void Draw(SpriteBatch batch)
        {
        }

        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                IsExiting = true;
            }
        }
    }
}
