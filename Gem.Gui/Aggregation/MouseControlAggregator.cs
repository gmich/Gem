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

        private void CheckMouseHover(AControl entry, AggregationToken token, int entryId)
        {
            var currentHasHover = entry.Region.Frame.Contains(input.MousePosition);

            if (currentHasHover && !token.HasHover)
            {
                entry.HasHover = true;
            }
            else if (!currentHasHover && token.HasHover)
            {
                entry.HasHover = false;
            }

            token.HasHover = currentHasHover;
            //if (token.IsSelected) return;
        }

        private void CheckFocus(AControl entry, AggregationToken token, int entryId)
        {
            if (input.IsLeftButtonClicked())
            {
                if (token.HasHover && !entry.HasFocus)
                {
                    entry.HasFocus = true;
                    token.GotFocusBy = this;
                }
                else if (!token.HasHover && entry.HasFocus)
                {
                    entry.HasFocus = false;
                    token.GotFocusBy = null;
                }
            }
            CheckSelected(entry, token, entryId);
        }

        private void CheckSelected(AControl entry, AggregationToken token, int entryId)
        {
            if (input.IsLeftButtonClicked())
            {
                if (token.HasHover)
                {
                    token.CanBeClicked = true;
                }
            }
            if (!input.IsLeftButtonPressed())
            {
                token.CanBeClicked = false;
            }
        }

        private void CheckClick(AControl entry, AggregationToken token, int entryId)
        {
            //Console.WriteLine(token.CanBeClicked);
            if (token.CanBeClicked 
                && token.HasHover 
                && input.IsLeftButtonReleased())
            {
                entry.Events.OnClicked();
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

            //TODO: add aggregation context to the area
            //Aggregate(area);

            int entryId = 0;

            Aggregate(entry.Control, entry.Token, entryId);

        }

        private void Aggregate(AControl entry, AggregationToken token, int entryId)
        {
            if (!entry.Options.IsEnabled || !this.IsEnabled)
            {
                return;
            }

            CheckMouseHover(entry, token, entryId);

            CheckClick(entry, token, entryId);

            if (entry.Options.IsFocusEnabled)
            {
                CheckFocus(entry, token, entryId);
            }

        }

        #endregion

    }
}
