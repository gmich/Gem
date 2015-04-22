using System;
using Gem.Gui.Controls.Aggregators;
using Gem.Gui.Controls.Handlers;
using Gem.Gui.Elements;
using Gem.Gui.Input;

namespace Gem.Gui.Handlers
{
    internal class MouseControlHandler<TEventArgs> : IControlHandler
        where TEventArgs : EventArgs
    {

        #region Readonly Fields

        private readonly DesktopControlAggregator<TEventArgs> control;
        private readonly Func<IGuiElement, TEventArgs> eventArgsProvider;
        private readonly DesktopInputHelper inputHandler;

        #endregion

        #region Ctor

        public MouseControlHandler(Func<IGuiElement, TEventArgs> eventArgsProvider,
                                   DesktopControlAggregator<TEventArgs> control,
                                   DesktopInputHelper inputHandler)
        {
            this.control = control;
            this.eventArgsProvider = eventArgsProvider;
            this.inputHandler = inputHandler;
        }

        #endregion

        #region Event Aggregation

        private bool hasHover = false;
        private void CheckMouseHover(IGuiElement element)
        {
            var currentHasHover = element.Region.Frame.Contains(inputHandler.MousePosition);

            if (currentHasHover == true
                && hasHover == false)
            {
                control.OnMouseCapture(eventArgsProvider(element));
            }
            else if (currentHasHover == false
                     && hasHover == true)
            {
                control.OnLostMouseCapture(eventArgsProvider(element));
            }

            hasHover = currentHasHover;
        }

        private bool gotFocus = false;
        private bool isSelected = false;
        private void CheckFocus(IGuiElement element)
        {
            var currentFocus = !gotFocus ?
                                hasHover && inputHandler.IsLeftButtonClicked():
                                inputHandler.IsLeftButtonPressed();

            if (currentFocus == true
                && gotFocus == false)
            {
                control.OnGotFocus(eventArgsProvider(element));
            }
            else if (currentFocus == false
                    && gotFocus == true)
            {
                control.OnLostFocus(eventArgsProvider(element));
            }
            gotFocus = currentFocus;

            isSelected = gotFocus && hasHover;
        }

        private void CheckClick(IGuiElement element)
        {
            if(isSelected && inputHandler.IsLeftButtonReleased())
            {
                control.OnClicked(eventArgsProvider(element));
            }
        }

        #endregion

        #region IControlHandler Members

        public void Update(IGuiElement element)
        {
            CheckMouseHover(element);
            CheckFocus(element);
            CheckClick(element);
        }

        #endregion

    }
}
