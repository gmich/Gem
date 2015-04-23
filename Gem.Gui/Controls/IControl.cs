using Gem.Gui.Elements;
using System;

namespace Gem.Gui.Controls
{
    public interface IControl<T> where T:EventArgs
    {
        event EventHandler<T> GotFocus;
        event EventHandler<T> LostFocus;
        event EventHandler<T> Clicked;
        event EventHandler<T> GotMouseCapture;
        event EventHandler<T> LostMouseCapture;
        event EventHandler<T> DragEnter;
        event EventHandler<T> DragLeave;
    }
}
