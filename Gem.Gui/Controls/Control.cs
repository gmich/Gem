using System;

namespace Gem.Gui.Controls
{
    public class Control<TEventArgs> : IControl<TEventArgs> 
        where TEventArgs : EventArgs
    {

        #region Events

        public event EventHandler<TEventArgs> LostFocus;

        public event EventHandler<TEventArgs> GotFocus;

        public event EventHandler<TEventArgs> Clicked;

        public event EventHandler<TEventArgs> GotMouseCapture;

        public event EventHandler<TEventArgs> LostMouseCapture;
        
        public event EventHandler<TEventArgs> DragEnter;

        public event EventHandler<TEventArgs> DragLeave;

        #endregion

        #region Aggregation

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
