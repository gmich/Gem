using System;
using Gem.Gui.Input;
using Gem.Gui.Controls;
using Microsoft.Xna.Framework;

namespace Gem.Gui.Aggregation
{
    internal class MouseControlAggregator : IAggregator
    {

        #region  Fields

        private readonly MouseInputHelper input;

        //this is used to spot new mouse locations
        private Point mouseLocation;

        #endregion

        public MouseControlAggregator(MouseInputHelper inputHelper)
        {
            this.input = inputHelper;
        }


        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                mouseLocation = input.MousePosition;
            }
        }

        #region Event Raising

        private void CheckMouseHover(GuiEntry entry, AggregationContext context)
        {
            var currentHasHover = entry.Control.Region.Frame.Contains(input.MousePosition);

            if (currentHasHover && !entry.Token.HasHover)
            {
                entry.Control.HasHover = true;
            }
            else if (!currentHasHover && entry.Token.HasHover)
            {
                entry.Control.HasHover = false;
            }

            entry.Token.HasHover = currentHasHover;
        }

        private void CheckFocus(GuiEntry entry, AggregationContext context)
        {
            if (input.IsLeftButtonClicked())
            {
                if (entry.Token.HasHover && !entry.Control.HasFocus)
                {
                    context.FocusControlAt(entry.Index);
                    entry.Token.GotFocusBy = this;
                }
                else if (!entry.Token.HasHover && entry.Control.HasFocus)
                {
                    context.Reset();
                    entry.Token.GotFocusBy = null;
                }
            }
            CheckSelected(entry, context);
        }

        private void CheckSelected(GuiEntry entry, AggregationContext context)
        {
            if (input.IsLeftButtonClicked())
            {
                if (entry.Token.HasHover)
                {
                    entry.Token.CanBeClicked = true;
                }
            }
            if (!input.IsLeftButtonPressed())
            {
                entry.Token.CanBeClicked = false;
            }
        }

        private void CheckClick(GuiEntry entry, AggregationContext context)
        {
            if (entry.Token.CanBeClicked 
                && entry.Token.HasHover 
                && input.IsLeftButtonReleased())
            {
                entry.Control.Events.OnClicked();
            }
        }

        #endregion

        #region Aggregation

        public void Aggregate(GuiEntry entry, AggregationContext context)
        {
            if (!this.IsEnabled)
            {
                //if the mouse has moved, then the aggregator is activated
                IsEnabled = (mouseLocation != input.MousePosition);
            }

            if (!entry.Control.Options.IsEnabled || !this.IsEnabled)
            {
                return;
            }

            CheckMouseHover(entry, context);

            CheckClick(entry, context);

            if (entry.Control.Options.IsFocusEnabled)
            {
                CheckFocus(entry, context);
            }

        }


        #endregion

    }
}
