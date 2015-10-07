using Gem.Engine.Input;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics.Contracts;
using Gem.Infrastructure.Events;

namespace Gem.Engine.Console.EntryPoint
{
    public class TextAppenderHelper
    {
        private readonly KeyboardInput input;
        private readonly char cursor;

        public TextAppenderHelper(KeyboardInput input,
                                  char cursor = '|',
                                  double cursorFlickInterval = 500.0d,
                                  double keyRepeatStartDuration = 0.5d,
                                  double keyRepeatDuration = 0.04d)
        {
            this.input = input;
            this.KeyRepeatDuration = keyRepeatDuration;
            this.KeyRepeatStartDuration = keyRepeatStartDuration;
            this.ShouldHandleKey = (key, charRepresentation) => true;
            this.cursor = cursor;
            this.CursorFlickInterval = cursorFlickInterval;
        }

        public double KeyRepeatStartDuration { get; set; }
        public double KeyRepeatDuration { get; set; }
        public double KeyRepeatTimer { get; set; }
        public double CursorFlickInterval { get; set; }

        public Func<Keys, char, bool> ShouldHandleKey { get; set; }

        public KeyboardInput Input { get { return input; } }

        public char Cursor { get { return cursor; } }

        public static TextAppenderHelper Default
        {
            get
            {
                //FIXME
                return new TextAppenderHelper(null);
            }
        }
    }

    public class KeyProcessor
    {
        private readonly TextAppenderHelper appender;
        private Keys pressedKey;

        #region Events

        public event EventHandler<char> KeyPressed;
        public event EventHandler<EventArgs> BackSpace;
        public event EventHandler<EventArgs> Delete;
        public event EventHandler<EventArgs> Left;
        public event EventHandler<EventArgs> Right;
        public event EventHandler<EventArgs> Up;
        public event EventHandler<EventArgs> Down;

        #endregion

        #region Ctor

        public KeyProcessor(TextAppenderHelper appender)
        {
            Contract.Requires(appender != null);
            this.appender = appender;
        }

        #endregion

        public void ProcessKeyInput(double deltaTime)
        {
            var pressedKeys = appender.Input.GetPressedKeys();

            bool isShiftPressed = appender.Input.IsKeyPressed(Keys.LeftShift) ||
                                  appender.Input.IsKeyPressed(Keys.RightShift);


            foreach (Keys key in pressedKeys)
            {
                if (!ShouldHandleKey(key, deltaTime)) continue;

                char convertedChar;
                if (KeyboardUtils.KeyToString(key, isShiftPressed, out convertedChar))
                {
                    if (!appender.ShouldHandleKey(key, convertedChar))
                    {
                        continue;
                    }
                    KeyPressed.RaiseEvent(this, convertedChar);
                }
                else
                {
                    switch (key)
                    {
                        case Keys.Back:
                            BackSpace.RaiseEvent(this, EventArgs.Empty);
                            break;
                        case Keys.Delete:
                            Delete.RaiseEvent(this, EventArgs.Empty);
                            break;
                        case Keys.Left:
                            Left.RaiseEvent(this, EventArgs.Empty);
                            break;
                        case Keys.Right:
                            Right.RaiseEvent(this, EventArgs.Empty);
                            break;
                        case Keys.Up:
                            Up.RaiseEvent(this, EventArgs.Empty);
                            break;
                        case Keys.Down:
                            Down.RaiseEvent(this, EventArgs.Empty);
                            break;
                    }
                }
            }
        }

        private bool ShouldHandleKey(Keys key, double timeDelta)
        {
            if (appender.Input.IsKeyClicked(key))
            {
                appender.KeyRepeatTimer = appender.KeyRepeatStartDuration;
                pressedKey = key;
                return true;
            }

            if (key == pressedKey)
            {
                appender.KeyRepeatTimer -= timeDelta;
                if (appender.KeyRepeatTimer <= 0.0f)
                {
                    appender.KeyRepeatTimer += appender.KeyRepeatDuration;
                    return true;
                }
            }
            return false;
        }
    }
}
