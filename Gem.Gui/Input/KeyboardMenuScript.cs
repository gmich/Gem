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
        }

        [DefaultValue(Keys.Down)]
        public Keys Next { get; set; }

        [DefaultValue(Keys.Up)]
        public Keys Previous { get; set; }

        [DefaultValue(Keys.Enter)]
        public Keys Trigger { get; set; }

    }
}
