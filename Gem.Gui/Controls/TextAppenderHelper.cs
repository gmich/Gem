using Gem.Gui.Input;
using Microsoft.Xna.Framework.Input;
using System;

namespace Gem.Gui.Controls
{
    public class TextAppenderHelper
    {
        private readonly KeyboardInputHelper input;
        private readonly char cursor;

        public TextAppenderHelper(KeyboardInputHelper input,
                                  char cursor = '_',
                                  double cursorFlickInterval = 1.0d,
                                  double keyRepeatStartDuration = 0.3d,
                                  double keyRepeatDuration = 0.003d)
        {
            this.input = input;
            this.KeyRepeatDuration = keyRepeatDuration;
            this.KeyRepeatStartDuration = keyRepeatStartDuration;
            this.ShouldHandleKey = key => true;
            this.cursor = cursor;
            this.CursorFlickInterval = cursorFlickInterval;
        }

        public double KeyRepeatStartDuration { get; set; }
        public double KeyRepeatDuration { get; set; }
        public double KeyRepeatTimer { get; set; }
        public double CursorFlickInterval { get; set; }

        public Predicate<Keys> ShouldHandleKey { get; set; }

        public KeyboardInputHelper Input { get { return input; } }

        public char Cursor { get { return cursor; } }

        public static TextAppenderHelper Default
        {
            get
            {
                return new TextAppenderHelper(InputManager.Keyboard);
            }
        }
    }
}
