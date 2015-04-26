using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;

namespace Gem.Gui.Input
{
    public static class InputManager
    {

        #region Fields

        private static PlayerIndex defaultGamePad;
        private static InputCapabilities inputCaps;

        #endregion

        #region Ctor

        static InputManager()
        {
            GetInputCapabilities(true);
        }

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
            keyboardHelper =
                inputCaps.IsKeyboardEnabled ?
                keyboardHelper ?? new KeyboardInputHelper()
                : null;

            mouseHelper =
                inputCaps.IsMouseEnabled ?
                mouseHelper ?? new MouseInputHelper()
                : null;

            touchHelper =
                inputCaps.IsTouchEnabled ?
                touchHelper ?? new TouchInputHelper()
                : null;

            gamePadHelper =
                inputCaps.IsGamePadEnabled ?
                gamePadHelper ?? new GamePadInputHelper()
                : null;
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

        private static KeyboardInputHelper keyboardHelper;
        public static KeyboardInputHelper Keyboard
        {
            get
            {
                return keyboardHelper;
            }
        }

        private static MouseInputHelper mouseHelper;
        public static MouseInputHelper Mouse
        {
            get
            {
                return mouseHelper;
            }
        }

        private static TouchInputHelper touchHelper;
        public static TouchInputHelper Touch
        {
            get
            {
                return touchHelper;
            }
        }

        private static GamePadInputHelper gamePadHelper;
        public static GamePadInputHelper GamePad
        {
            get
            {
                return gamePadHelper;
            }
        }

        #endregion

        #region Update

        public static void Update()
        {
            FlushDesktop();
            Flush(touchHelper);
        }

        [Conditional("WINDOWS"), Conditional("LINUX")]
        private static void FlushDesktop()
        {
            Flush(keyboardHelper);
            Flush(mouseHelper);
            Flush(gamePadHelper);
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
