using Gem.Gui.Elements;
using System;

namespace Gem.Gui.Controls
{
    public class ControlAggregator<TEventArgs> : IControl<TEventArgs>
        where TEventArgs : EventArgs
    {

        #region Events
        
        public event EventHandler<TEventArgs> LostFocus;

        public event EventHandler<TEventArgs> GotFocus;

        public event EventHandler<TEventArgs> Clicked;
        
        #endregion

        #region Aggregation

        internal void OnGotFocus(TEventArgs args)
        {
            var handler = GotFocus;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        internal void OnLostFocus(TEventArgs args)
        {
            var handler = LostFocus;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        internal void OnClicked(TEventArgs args)
        {
            var handler = Clicked;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        #endregion

    }
}
