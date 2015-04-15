using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Controls
{
    public interface ICommonGuiEvent<T> where T:EventArgs
    {
        event EventHandler<T> HasFocus;
        event EventHandler<T> LostFocus;
        event EventHandler<T> Clicked;
        event EventHandler<T> Released;   
    }

    public interface ISliderEvent<T> where T : EventArgs
    {
        event EventHandler<T> DragEnter;
        event EventHandler<T> DragLeave;
    }
}
