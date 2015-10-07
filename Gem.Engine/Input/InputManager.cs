using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Gem.Infrastructure.Input;
using System;
using System.Diagnostics;

namespace Gem.Engine.Input
{
    public class InputManager 
    {

        #region Fields

        private InputCapabilities inputCaps;

        #endregion

        #region Input Capabilities

        public struct InputCapabilities
        {
            public bool IsGamePadEnabled { get; internal set; }
            public bool IsKeyboardEnabled { get; internal set; }
            public bool IsMouseEnabled { get; internal set; }
            public bool IsTouchEnabled { get; internal set; }
        }

        public InputCapabilities GetInputCapabilities(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                RunningOnPhone();
                RunningOnDesktop();
                inputCaps.IsTouchEnabled = TouchPanel.GetCapabilities().IsConnected;
            }

            return inputCaps;
        }

        [Conditional("ANDROID"), Conditional("WINDOWS_PHONE")]
        private void RunningOnPhone()
        {
            inputCaps.IsGamePadEnabled = false;
            inputCaps.IsMouseEnabled = false;
            inputCaps.IsKeyboardEnabled = false;
        }

        [Conditional("WINDOWS"), Conditional("LINUX")]
        private void RunningOnDesktop()
        {
            inputCaps.IsGamePadEnabled = Microsoft.Xna.Framework.Input.GamePad.GetCapabilities(PlayerIndex.One).IsConnected;
            inputCaps.IsMouseEnabled = true;
            inputCaps.IsKeyboardEnabled = true;
        }

        #endregion

        #region Public Properties

        private Lazy<KeyboardInput> keyboard = new Lazy<KeyboardInput>();
        public KeyboardInput Keyboard
        {
            get
            {
                return keyboard.Value;
            }
        }

        private Lazy<MouseInput> mouse = new Lazy<MouseInput>();
        public MouseInput Mouse
        {
            get
            {
                return mouse.Value;
            }
        }

        private Lazy<TouchInput> touch = new Lazy<TouchInput>();
        public TouchInput Touch
        {
            get
            {
                return touch.Value;
            }
        }

        private Lazy<GamePadInput> gamePad = new Lazy<GamePadInput>();
        public GamePadInput GamePad
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

        public void Flush()
        {
            FlushDesktop();
        }

        private void FlushDesktop()
        {
            Flush(Keyboard);
            Flush(Mouse);
            Flush(GamePad);
        }

        private void Flush(IInput input)
        {
            if (input != null)
            {
                input.Flush();
            }
        }

        #endregion



    }
}

