using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Diagnostics;

namespace Gem.Gui.Input
{
    public class InputManager : GameComponent
    {

        #region Fields

        private static PlayerIndex defaultGamePad = PlayerIndex.One;
        private static InputCapabilities inputCaps;

        #endregion

        #region Ctor

        public InputManager(Game game):base(game)
        {
            //GetInputCapabilities(true);
        }

        #endregion

        #region Scripts

        public static KeyboardMenuScript KeyboardMenuScript { get; set; }

        #endregion

        #region Input Capabilities

        public struct InputCapabilities
        {
            public bool IsGamePadEnabled { get; internal set; }
            public bool IsKeyboardEnabled { get; internal set; }
            public bool IsMouseEnabled { get; internal set; }
            public bool IsTouchEnabled { get; internal set; }
        }

        public static InputCapabilities GetInputCapabilities(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                RunningOnPhone();
                RunningOnDesktop();
                inputCaps.IsTouchEnabled = TouchPanel.GetCapabilities().IsConnected;
                EvaluateInputDevices();
            }

            return inputCaps;
        }

        private static void EvaluateInputDevices()
        {
            //keyboardHelper =
            //    inputCaps.IsKeyboardEnabled ?
            //    keyboardHelper ?? new KeyboardInputHelper()
            //    : null;

            //mouseHelper =
            //    inputCaps.IsMouseEnabled ?
            //    mouseHelper ?? new MouseInputHelper()
            //    : null;

            //touchHelper =
            //    inputCaps.IsTouchEnabled ?
            //    touchHelper ?? new TouchInputHelper()
            //    : null;

            //gamePadHelper =
            //    inputCaps.IsGamePadEnabled ?
            //    gamePadHelper ?? new GamePadInputHelper()
            //    : null;
        }

        [Conditional("ANDROID"), Conditional("WINDOWS_PHONE")]
        private static void RunningOnPhone()
        {
            inputCaps.IsGamePadEnabled = false;
            inputCaps.IsMouseEnabled = false;
            inputCaps.IsKeyboardEnabled = false;
        }

        [Conditional("WINDOWS"), Conditional("LINUX")]
        private static void RunningOnDesktop()
        {
            inputCaps.IsGamePadEnabled = Microsoft.Xna.Framework.Input.GamePad.GetCapabilities(defaultGamePad).IsConnected;
            inputCaps.IsMouseEnabled = true;
            inputCaps.IsKeyboardEnabled = true;
        }

        #endregion

        #region Public Properties

        private static Lazy<KeyboardInputHelper> keyboardHelper = new Lazy<KeyboardInputHelper>();
        public static KeyboardInputHelper Keyboard
        {
            get
            {
                return keyboardHelper.Value;
            }
        }

        private static Lazy<MouseInputHelper> mouseHelper = new Lazy<MouseInputHelper>();
        public static MouseInputHelper Mouse
        {
            get
            {
                return mouseHelper.Value;
            }
        }

        private static Lazy<TouchInputHelper> touchHelper = new Lazy<TouchInputHelper>();
        public static TouchInputHelper Touch
        {
            get
            {
                return touchHelper.Value;
            }
        }

        private static Lazy<GamePadInputHelper> gamePadHelper = new Lazy<GamePadInputHelper>();
        public static GamePadInputHelper GamePad
        {
            get
            {
                return gamePadHelper.Value;
            }
        }

        #endregion

        #region GameComponent Members

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            FlushDesktop();
            //Flush(Touch);
        }

        //[Conditional("WINDOWS"), Conditional("LINUX")]
        private static void FlushDesktop()
        {
            Flush(Keyboard);
            Flush(Mouse);
            //Flush(GamePad);
        }

        private static void Flush(IInputHelper inputHelper)
        {
            if (inputHelper != null)
            {
                inputHelper.Flush();
            }
        }

        #endregion



    }
}
