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
        private readonly Func<IGuiComponent, TEventArgs> eventArgsProvider;
        private readonly MouseInputHelper inputHandler;
        
        #endregion

        public event EventHandler OnActivated;
        public event EventHandler OnDeactivated;

        #region Ctor

        public MouseControlAggregator(Func<IGuiComponent, TEventArgs> eventArgsProvider,
                                      Control<TEventArgs> control,
                                      MouseInputHelper inputHandler)
        {
            this.eventArgsProvider = eventArgsProvider;
            this.control = control;
            this.inputHandler = inputHandler;
        }

        #endregion

        #region Event Raising

        private bool hasHover = false;
        private void CheckMouseHover(IGuiComponent element)
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
        private void CheckFocus(IGuiComponent element)
        {
            var currentFocus = !gotFocus ?
                                hasHover && inputHandler.IsLeftButtonClicked() :
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

        private void CheckClick(IGuiComponent element)
        {
            if (isSelected && inputHandler.IsLeftButtonReleased())
            {
                control.OnClicked(eventArgsProvider(element));
            }
        }

        #endregion

        #region Aggregation

        public void Aggregate(IGuiComponent element, AggregationToken context)
        {
            if (!element.Options.IsEnabled && !context.Captured)
            {
                return;
            }
            CheckMouseHover(element);

            if (element.Options.IsFocusEnabled)
            {
                CheckFocus(element);
            }

            CheckClick(element);
        }

        #endregion

    }
}
