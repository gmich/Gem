using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Gem.Infrastructure.Input;
using System;
using System.Diagnostics;

namespace Gem.Gui.Input
{
    public class InputManager : GameComponent
    {

        #region Fields

        private static InputCapabilities inputCaps;

        #endregion

        #region Ctor

        public InputManager(Game game)
            : base(game)
        {
            //GetInputCapabilities(true);
        }

        static InputManager()
        {
            KeyboardInputKeys = new KeyboardInputKeys();
            GamePadInputKeys = new GamePadInputButtons();
            KeyRepetition = new KeyRepetition();
        }

        #endregion

        #region Scripts

        public static KeyboardInputKeys KeyboardInputKeys { get; private set; }

        public static GamePadInputButtons GamePadInputKeys { get; private set; }
        
        public static KeyRepetition KeyRepetition { get; private set; }

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
            //keyboard =
            //    inputCaps.IsKeyboardEnabled ?
            //    keyboard ?? new KeyboardInput()
            //    : null;

            //mouse =
            //    inputCaps.IsMouseEnabled ?
            //    mouse ?? new MouseInput()
            //    : null;

            //touch =
            //    inputCaps.IsTouchEnabled ?
            //    touch ?? new TouchInput()
            //    : null;

            //gamePad =
            //    inputCaps.IsGamePadEnabled ?
            //    gamePad ?? new GamePadInput()
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
            inputCaps.IsGamePadEnabled = Microsoft.Xna.Framework.Input.GamePad.GetCapabilities(GamePadInputKeys.PlayerIndex).IsConnected;
            inputCaps.IsMouseEnabled = true;
            inputCaps.IsKeyboardEnabled = true;
        }

        #endregion

        #region Public Properties

        private static Lazy<KeyboardInput> keyboard = new Lazy<KeyboardInput>();
        public static KeyboardInput Keyboard
        {
            get
            {
                return keyboard.Value;
            }
        }

        private static Lazy<MouseInput> mouse = new Lazy<MouseInput>();
        public static MouseInput Mouse
        {
            get
            {
                return mouse.Value;
            }
        }

        private static Lazy<TouchInput> touch = new Lazy<TouchInput>();
        public static TouchInput Touch
        {
            get
            {
                return touch.Value;
            }
        }

        private static Lazy<GamePadInput> gamePad = new Lazy<GamePadInput>();
        public static GamePadInput GamePad
        {
            get
            {
                return gamePad.Value;
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
            Flush(GamePad);
        }

        private static void Flush(IInput input)
        {
            if (input != null)
            {
                input.Flush();
            }
        }

        #endregion



    }
}
