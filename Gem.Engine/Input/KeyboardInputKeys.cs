using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;

namespace Gem.Input
{
    public class KeyboardInputKeys
    {
        public KeyboardInputKeys()
        {
            this.AssignDefaultValues();
        }

        [DefaultValue(Keys.Right)]
        public Keys Right { get; set; }

        [DefaultValue(Keys.Left)]
        public Keys Left { get; set; }

        [DefaultValue(Keys.Down)]
        public Keys Next { get; set; }

        [DefaultValue(Keys.Up)]
        public Keys Previous { get; set; }

        [DefaultValue(Keys.Enter)]
        public Keys Trigger { get; set; }

    }


}
