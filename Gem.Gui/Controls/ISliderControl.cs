using System;

namespace Gem.Gui.Controls
{
    public interface ISliderControl<T> : IDesktopControl<T> 
        where T : EventArgs
    {
        event EventHandler<T> DragEnter;
        event EventHandler<T> DragLeave;
    }
}
