using System;
using Gem.Gui.Input;
using Gem.Gui.Controls;
using Microsoft.Xna.Framework;
using Gem.Infrastructure.Input;

namespace Gem.Gui.Aggregation
{
    /// <summary>
    /// Aggregates controls according to mouse input
    /// </summary>
    internal class MouseControlAggregator : IAggregator
    {

        #region  Fields

        /// <summary>
        /// The helper class that sends mouse signals
        /// </summary>
        private readonly MouseInput input;

        #endregion

        public MouseControlAggregator(MouseInput inputHelper)
        {
            this.input = inputHelper;
            this.IsEnabled = true;
        }

        public bool IsEnabled
        {
            get;
            private set;
        }

        #region Event Raising

        private void CheckMouseHover(AggregationEntry entry, AggregationContext context)
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

        private void CheckFocus(AggregationEntry entry, AggregationContext context)
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

        private void CheckSelected(AggregationEntry entry, AggregationContext context)
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

        private void CheckClick(AggregationEntry entry, AggregationContext context)
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

        public void Aggregate(AggregationEntry entry, AggregationContext context)
        {
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
