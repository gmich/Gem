namespace Gem.Gui.Input
{
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
