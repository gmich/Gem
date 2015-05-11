using Gem.Gui.Core.Styles;
using System;

namespace Gem.Gui.Events
{
    public class ViewEvents<TEventArgs>
        where TEventArgs : EventArgs
    {

        private readonly object sender;
        private readonly Func<TEventArgs> eventArgsProvider;

        public ViewEvents(object sender, Func<TEventArgs> eventArgsProvider)
        {
            this.sender = sender;
            this.eventArgsProvider = eventArgsProvider;
        }

        #region Events

        public event EventHandler<TEventArgs> LostFocus;

        public event EventHandler<TEventArgs> GotFocus;

        public event EventHandler<TEventArgs> Clicked;

        public event EventHandler<TEventArgs> GotMouseCapture;

        public event EventHandler<TEventArgs> LostMouseCapture;
        
        #endregion

        #region Helpers

        internal void SubscribeStyle(IRenderStyle style)
        {
            this.Clicked += (sender, args) => style.Clicked();
            this.GotFocus += (sender, args) => style.Focus();
            this.LostFocus += (sender, args) => style.Default();
            this.LostMouseCapture += (sender, args) => style.Default();
            this.GotMouseCapture += (sender, args) => style.Hover();
            style.Default();
        }

        #endregion

        #region Aggregation

        internal void OnMouseCapture()
        {
            var handler = GotMouseCapture;
            if (handler != null)
            {
                handler(sender, eventArgsProvider());
            }
        }

        internal void OnLostMouseCapture()
        {
            var handler = LostMouseCapture;
            if (handler != null)
            {
                handler(sender, eventArgsProvider());
            }
        }

        internal void OnGotFocus()
        {
            var handler = GotFocus;
            if (handler != null)
            {
                handler(sender, eventArgsProvider());
            }
        }

        internal void OnLostFocus()
        {
            var handler = LostFocus;
            if (handler != null)
            {
                handler(sender, eventArgsProvider());
            }
        }

        internal void OnClicked()
        {
            var handler = Clicked;
            if (handler != null)
            {
                handler(sender, eventArgsProvider());
            }
        }

        #endregion

    }
}
