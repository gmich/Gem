using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Gem.DrawingSystem;
using Gem.ScreenSystem;

namespace Gem.Input
{
    /// <summary>
    ///   an enum of all available mouse buttons.
    /// </summary>
    public enum MouseButtons
    {
        LeftButton,
        MiddleButton,
        RightButton,
        ExtraButton1,
        ExtraButton2
    }

    /// <summary>
    /// A ScreenManager specific multiplatform input helper.
    /// Notice: The current InputHelper cannot exists without the ScreenManager.
    /// </summary>
    public class InputHelper
    {

        #region Private Fields

        private readonly List<GestureSample> gestures = new List<GestureSample>();

        private bool handleVirtualStick;

        private bool cursorIsVisible;
        private Sprite cursorSprite;

#if WINDOWS_PHONE
        private VirtualStick _phoneStick;
        private VirtualButton _phoneA;
        private VirtualButton _phoneB;
#endif

        private readonly ScreenManager manager;
        private Viewport viewport;

        #endregion


        #region Constructor

        /// <summary>
        ///   Constructs a new input state.
        /// </summary>
        public InputHelper(ScreenManager manager)
        {
            KeyboardState = new KeyboardState();
            GamePadState = new GamePadState();
            MouseState = new MouseState();
            VirtualState = new GamePadState();

            PreviousKeyboardState = new KeyboardState();
            PreviousGamePadState = new GamePadState();
            PreviousMouseState = new MouseState();
            PreviousVirtualState = new GamePadState();

            this.manager = manager;

            cursorIsVisible = false;
            IsCursorMoved = false;
#if WINDOWS_PHONE
            IsCursorValid = false;
#else
            IsCursorValid = true;
#endif
            Cursor = Vector2.Zero;

            handleVirtualStick = false;
        }


        #endregion


        #region Public Properties


        public GamePadState GamePadState { get; private set; }

        public KeyboardState KeyboardState { get; private set; }

        public MouseState MouseState { get; private set; }

        public GamePadState VirtualState { get; private set; }

        public GamePadState PreviousGamePadState { get; private set; }

        public KeyboardState PreviousKeyboardState { get; private set; }

        public MouseState PreviousMouseState { get; private set; }

        public GamePadState PreviousVirtualState { get; private set; }

        public bool ShowCursor
        {
            get { return cursorIsVisible && IsCursorValid; }
            set { cursorIsVisible = value; }
        }

        public bool EnableVirtualStick
        {
            get { return handleVirtualStick; }
            set { handleVirtualStick = value; }
        }

        public Vector2 Cursor { get; private set; }

        public bool IsCursorMoved { get; private set; }

        public bool IsCursorValid { get; private set; }


        #endregion


        #region Monogame Architecture Related


        public void LoadContent()
        {
            cursorSprite = new Sprite(manager.Content.Load<Texture2D>("Common/cursor"));
#if WINDOWS_PHONE
            // virtual stick content
            _phoneStick = new VirtualStick(_manager.Content.Load<Texture2D>("Common/socket"),
                                           _manager.Content.Load<Texture2D>("Common/stick"), new Vector2(80f, 400f));

            Texture2D temp = _manager.Content.Load<Texture2D>("Common/buttons");
            _phoneA = new VirtualButton(temp, new Vector2(695f, 380f), new Rectangle(0, 0, 40, 40), new Rectangle(0, 40, 40, 40));
            _phoneB = new VirtualButton(temp, new Vector2(745f, 360f), new Rectangle(40, 0, 40, 40), new Rectangle(40, 40, 40, 40));
#endif
            viewport = manager.GraphicsDevice.Viewport;
        }

        /// <summary>
        ///   Reads the latest state of the keyboard and gamepad and mouse/touchpad.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            PreviousKeyboardState = KeyboardState;
            PreviousGamePadState = GamePadState;
            PreviousMouseState = MouseState;

            if (handleVirtualStick)
                PreviousVirtualState = VirtualState;

            KeyboardState = Keyboard.GetState();
            GamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState = Mouse.GetState();

            if (handleVirtualStick)
            {
#if XBOX
                VirtualState = GamePad.GetState(PlayerIndex.One);
#elif WINDOWS
                VirtualState = GamePad.GetState(PlayerIndex.One).IsConnected ? GamePad.GetState(PlayerIndex.One) : HandleVirtualStickWin();
#elif WINDOWS_PHONE
                VirtualState = HandleVirtualStickWP7();
#endif
            }

            gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                gestures.Add(TouchPanel.ReadGesture());
            }

            // Update cursor
            Vector2 oldCursor = Cursor;
            if (GamePadState.IsConnected && GamePadState.ThumbSticks.Left != Vector2.Zero)
            {
                Vector2 temp = GamePadState.ThumbSticks.Left;
                Cursor += temp * new Vector2(300f, -300f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Mouse.SetPosition((int)Cursor.X, (int)Cursor.Y);
            }
            else
            {
                Cursor = new Vector2(MouseState.X, MouseState.Y);
            }

            Cursor = new Vector2(MathHelper.Clamp(Cursor.X, 0f, viewport.Width), MathHelper.Clamp(Cursor.Y, 0f, viewport.Height));

            if (IsCursorValid && oldCursor != Cursor)
                IsCursorMoved = true;
            else
                IsCursorMoved = false;

#if WINDOWS
            IsCursorValid = viewport.Bounds.Contains(MouseState.X, MouseState.Y);
#elif WINDOWS_PHONE
            IsCursorValid = MouseState.LeftButton == ButtonState.Pressed;
#endif
        }

        public void Draw()
        {
            if (cursorIsVisible && IsCursorValid)
            {
                manager.SpriteBatch.Begin();
                manager.SpriteBatch.Draw(cursorSprite.Texture, Cursor, null, Color.White, 0f, cursorSprite.Origin, 1f, SpriteEffects.None, 0f);
                manager.SpriteBatch.End();
            }
#if WINDOWS_PHONE
            if (_handleVirtualStick)
            {
                _manager.SpriteBatch.Begin();
                _phoneA.Draw(_manager.SpriteBatch);
                _phoneB.Draw(_manager.SpriteBatch);
                _phoneStick.Draw(_manager.SpriteBatch);
                _manager.SpriteBatch.End();
            }
#endif
        }

        #endregion


        #region Handle Key Presses

        private GamePadState HandleVirtualStickWin()
        {
            Vector2 leftStick = Vector2.Zero;
            List<Buttons> buttons = new List<Buttons>();

            if (KeyboardState.IsKeyDown(Keys.A))
                leftStick.X -= 1f;
            if (KeyboardState.IsKeyDown(Keys.S))
                leftStick.Y -= 1f;
            if (KeyboardState.IsKeyDown(Keys.D))
                leftStick.X += 1f;
            if (KeyboardState.IsKeyDown(Keys.W))
                leftStick.Y += 1f;
            if (KeyboardState.IsKeyDown(Keys.Space))
                buttons.Add(Buttons.A);
            if (KeyboardState.IsKeyDown(Keys.LeftControl))
                buttons.Add(Buttons.B);
            if (leftStick != Vector2.Zero)
                leftStick.Normalize();

            return new GamePadState(leftStick, Vector2.Zero, 0f, 0f, buttons.ToArray());
        }

        private GamePadState HandleVirtualStickWP7()
        {
            List<Buttons> buttons = new List<Buttons>();
            Vector2 stick = Vector2.Zero;
#if WINDOWS_PHONE
            phoneA.Pressed = false;
            phoneB.Pressed = false;
            TouchCollection touchLocations = TouchPanel.GetState();
            foreach (TouchLocation touchLocation in touchLocations)
            {
                phoneA.Update(touchLocation);
                phoneB.Update(touchLocation);
                phoneStick.Update(touchLocation);
            }
            if (phoneA.Pressed)
            {
                buttons.Add(Buttons.A);
            }
            if (phoneB.Pressed)
            {
                buttons.Add(Buttons.B);
            }
            stick = phoneStick.StickPosition;
#endif
            return new GamePadState(stick, Vector2.Zero, 0f, 0f, buttons.ToArray());
        }

        /// <summary>
        ///   Helper for checking if a key was newly pressed during this update.
        /// </summary>
        public bool IsNewKeyPress(Keys key)
        {
            return (KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key));
        }

        public bool IsNewKeyRelease(Keys key)
        {
            return (PreviousKeyboardState.IsKeyDown(key) && KeyboardState.IsKeyUp(key));
        }

        /// <summary>
        ///   Helper for checking if a button was newly pressed during this update.
        /// </summary>
        public bool IsNewButtonPress(Buttons button)
        {
            return (GamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button));
        }

        public bool IsNewButtonRelease(Buttons button)
        {
            return (PreviousGamePadState.IsButtonDown(button) && GamePadState.IsButtonUp(button));
        }

        /// <summary>
        ///   Helper for checking if a mouse button was newly pressed during this update.
        /// </summary>
        public bool IsNewMouseButtonPress(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return (MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released);
                case MouseButtons.RightButton:
                    return (MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released);
                case MouseButtons.MiddleButton:
                    return (MouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released);
                case MouseButtons.ExtraButton1:
                    return (MouseState.XButton1 == ButtonState.Pressed && PreviousMouseState.XButton1 == ButtonState.Released);
                case MouseButtons.ExtraButton2:
                    return (MouseState.XButton2 == ButtonState.Pressed && PreviousMouseState.XButton2 == ButtonState.Released);
                default:
                    return false;
            }
        }


        /// <summary>
        /// Checks if the requested mouse button is released.
        /// </summary>
        /// <param name="button">The button.</param>
        public bool IsNewMouseButtonRelease(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LeftButton:
                    return (PreviousMouseState.LeftButton == ButtonState.Pressed && MouseState.LeftButton == ButtonState.Released);
                case MouseButtons.RightButton:
                    return (PreviousMouseState.RightButton == ButtonState.Pressed && MouseState.RightButton == ButtonState.Released);
                case MouseButtons.MiddleButton:
                    return (PreviousMouseState.MiddleButton == ButtonState.Pressed && MouseState.MiddleButton == ButtonState.Released);
                case MouseButtons.ExtraButton1:
                    return (PreviousMouseState.XButton1 == ButtonState.Pressed && MouseState.XButton1 == ButtonState.Released);
                case MouseButtons.ExtraButton2:
                    return (PreviousMouseState.XButton2 == ButtonState.Pressed && MouseState.XButton2 == ButtonState.Released);
                default:
                    return false;
            }
        }


        #endregion


        #region Menu Specific
        /// <summary>
        ///   Checks for a "menu select" input action.
        ///   Notice: The Key shuld not be hardcoded. Get them externally
        /// </summary>
        public bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter) || IsNewButtonPress(Buttons.A) || IsNewButtonPress(Buttons.Start) || IsNewMouseButtonPress(MouseButtons.LeftButton);
        }

        public bool IsMenuPressed()
        {
            return KeyboardState.IsKeyDown(Keys.Space) || KeyboardState.IsKeyDown(Keys.Enter) || GamePadState.IsButtonDown(Buttons.A) || GamePadState.IsButtonDown(Buttons.Start) || MouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsMenuReleased()
        {
            return IsNewKeyRelease(Keys.Space) || IsNewKeyRelease(Keys.Enter) || IsNewButtonRelease(Buttons.A) || IsNewButtonRelease(Buttons.Start) || IsNewMouseButtonRelease(MouseButtons.LeftButton);
        }

        /// <summary>
        ///   Checks for a "menu cancel" input action.
        ///   Notice: The Key shuld not be hardcoded. Get them externally
        /// </summary>
        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape) || IsNewButtonPress(Buttons.Back);
        }

        #endregion

    }
}