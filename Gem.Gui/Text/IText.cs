using System;

namespace Gem.Gui.Text
{
    public interface IText
    {
        event EventHandler<TextEventArgs> OnTextChanged;
       
        string Value { get; set; }

        Alignment.AlignmentContext Alignment { get; set; }
    }
}
