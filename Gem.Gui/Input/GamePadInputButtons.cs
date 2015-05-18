using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;
using Microsoft.Xna.Framework;

namespace Gem.Gui.Input
{
    public class GamePadInputButtons
    {
        public GamePadInputButtons()
        {
            this.AssignDefaultValues();
            this.KeyRepetition = new KeyRepetition();
        }

        [DefaultValue(Buttons.DPadDown)]
        public Buttons Next { get; set; }

        [DefaultValue(Buttons.DPadUp)]
        public Buttons Previous { get; set; }

        [DefaultValue(Buttons.A)]
        public Buttons Trigger { get; set; }

        [DefaultValue(PlayerIndex.One)]
        public PlayerIndex PlayerIndex { get; set; }

        public KeyRepetition KeyRepetition { get; private set; }
    }
    
}
