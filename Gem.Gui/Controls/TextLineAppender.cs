using Gem.Gui.Input;
using Microsoft.Xna.Framework.Input;
using System;

namespace Gem.Gui.Controls
{
    public class TextAppenderHelper
    {
        private readonly KeyboardInputHelper input;

        public TextAppenderHelper(KeyboardInputHelper input, double keyRepeatStartDuration = 0.3d, double keyRepeatDuration = 0.003d)
        {
            this.input = input;
            this.keyRepeatDuration = keyRepeatDuration;
            this.keyRepeatStartDuration = keyRepeatStartDuration;
            this.ShouldHandleKey = key => true;
        }

        public double keyRepeatStartDuration { get; set; }
        public double keyRepeatDuration { get; set; }
        public double keyRepeatTimer { get; set; }
        public Predicate<Keys> ShouldHandleKey { get; set; }
        public KeyboardInputHelper Input { get { return input; } }

        public static TextAppenderHelper Default
        {
            get
            {
                return new TextAppenderHelper(InputManager.Keyboard);
            }
        }
    }
}
