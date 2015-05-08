using System;

namespace Gem.Gui.Text
{
    public class TextEventArgs : EventArgs
    {
        public string PreviousText { get; set; }
        public string NewText { get; set; }
    }
}
