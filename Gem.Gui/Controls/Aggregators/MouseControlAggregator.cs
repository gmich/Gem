using System;
using Gem.Gui.Elements;
using Gem.Gui.Input;
using Gem.Gui.Controls;
using Gem.Gui.Controls.Aggregators;

namespace Gem.Gui.Aggregators
{
    internal class MouseControlAggregator<TEventArgs> : IControlAggregator
    where TEventArgs : EventArgs
    {

        #region Readonly Fields

        private readonly Control<TEventArgs> control;
        private readonly Func<IGuiElement, TEventArgs> eventArgsProvider;
        private readonly DesktopInputHelper inputHandler;

        #endregion
        
        #region Ctor

        public MouseControlAggregator(Func<IGuiElement, TEventArgs> eventArgsProvider,  
                                     Control<TEventArgs> control,            
                                     DesktopInputHelper inputHandler)
        {
            this.eventArgsProvider = eventArgsProvider;
            this.control = control;
            this.inputHandler = inputHandler;
        }

        #endregion


        #region Event Raising

        private bool hasHover = false;
        private void CheckMouseHover(IGuiElement element)
        {
            var currentHasHover = element.Region.Frame.Contains(inputHandler.MousePosition);

            if (currentHasHover && !hasHover)
            {
                control.OnMouseCapture(eventArgsProvider(element));
            }
            else if (!currentHasHover && hasHover)
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

            if (currentFocus && !gotFocus)
            {
                control.OnGotFocus(eventArgsProvider(element));
            }
            else if (!currentFocus && gotFocus)
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

        #region Aggregation

        public void Aggregate(IGuiElement element)
        {
            CheckMouseHover(element);
            CheckFocus(element);
            CheckClick(element);
        }

        #endregion

    }
}
