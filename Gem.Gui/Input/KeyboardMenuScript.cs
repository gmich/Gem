using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;

namespace Gem.Gui.Input
{
    public class KeyboardMenuScript
    {
        public KeyboardMenuScript()
        {
            this.AssignDefaultValues();
            this.KeyRepetition = new KeyRepetition();
        }

        [DefaultValue(Keys.Down)]
        public Keys Next { get; set; }

        [DefaultValue(Keys.Up)]
        public Keys Previous { get; set; }

        [DefaultValue(Keys.Enter)]
        public Keys Trigger { get; set; }

        public KeyRepetition KeyRepetition { get; private set; }
    }

    public class KeyRepetition
    {
        public KeyRepetition()
        {
            KeyRepeatStartDuration = 0.6d;
            KeyRepeatDuration = 0.2d;
        }

        public double KeyRepeatStartDuration { get; set; }
        public double KeyRepeatDuration { get; set; }
        public double KeyRepeatTimer { get; set; }
    }
}
