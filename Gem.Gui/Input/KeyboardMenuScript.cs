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

        [DefaultValue(Keys.Up)]
        public Keys Up { get; set; }

        [DefaultValue(Keys.Down)]
        public Keys Down { get; set; }

        [DefaultValue(Keys.Left)]
        public Keys Left { get; set; }

        [DefaultValue(Keys.Right)]
        public Keys Right { get; set; }

        [DefaultValue(Keys.Enter)]
        public Keys Trigger { get; set; }

        [DefaultValue(Keys.Escape)]
        public Keys Back { get; set; }
    }
}
