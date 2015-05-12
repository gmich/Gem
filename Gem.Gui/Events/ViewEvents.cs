using Gem.Gui.Controls;
using Gem.Gui.Styles;
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

        internal void SubscribeStyle(AControl control, IRenderStyle style)
        {
            this.Clicked += (sender, args) => style.Clicked(control);
            this.GotFocus += (sender, args) => style.Focus(control);
            this.LostFocus += (sender, args) => style.Default(control);
            this.LostMouseCapture += (sender, args) => style.Default(control);
            this.GotMouseCapture += (sender, args) => style.Hover(control);
            style.Default(control);
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
