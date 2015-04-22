using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Controls.Aggregators
{
    public class DesktopControlAggregator<TEventArgs> :
                 ControlAggregator<TEventArgs>, IDesktopControl<TEventArgs>
                 where TEventArgs : EventArgs
    {
        public event EventHandler<TEventArgs> GotMouseCapture;

        public event EventHandler<TEventArgs> LostMouseCapture;

        internal void OnMouseCapture(TEventArgs args)
        {
            var handler = GotMouseCapture;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        internal void OnLostMouseCapture(TEventArgs args)
        {
            var handler = LostMouseCapture;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}
