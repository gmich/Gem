using System;

namespace Gem.Gui.Controls
{
    public interface IDesktopControl<T> : IControl<T>
        where T : EventArgs
    {
        event EventHandler<T> GotMouseCapture;
        event EventHandler<T> LostMouseCapture;
    }

}
