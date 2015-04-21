using System;

namespace Gem.Gui.Controls
{
    public interface IControl<T> where T:EventArgs
    {
        event EventHandler<T> GotFocus;
        event EventHandler<T> LostFocus;
        event EventHandler<T> Clicked;   
    }
}
