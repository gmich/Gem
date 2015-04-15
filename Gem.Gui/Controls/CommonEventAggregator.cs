using System;

namespace Gem.Gui.Controls
{
    public class CommonEventAggregator<TEventArgs> : ICommonGuiEvent<TEventArgs>
        where TEventArgs : EventArgs
    {

        #region Events

        public event EventHandler<TEventArgs> HasFocus;

        public event EventHandler<TEventArgs> LostFocus;

        public event EventHandler<TEventArgs> Clicked;

        public event EventHandler<TEventArgs> Released;

        #endregion

        #region Aggregation

        internal void OnFocus(TEventArgs args)
        {
            var handler = HasFocus;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        internal void OnLostFocus(TEventArgs args)
        {
            var handler = HasFocus;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        internal void OnClicked(TEventArgs args)
        {
            var handler = HasFocus;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        internal void OnReleased(TEventArgs args)
        {
            var handler = HasFocus;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        #endregion
    }
}
