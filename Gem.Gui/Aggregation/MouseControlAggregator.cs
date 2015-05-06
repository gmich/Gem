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
                entry.Events.OnMouseCapture();
            }
            else if (!currentHasHover && token.HasHover)
            {
                entry.Events.OnLostMouseCapture();
            }

            token.HasHover = currentHasHover;
        }

        private void CheckFocus(AControl entry, AggregationToken token, int entryId)
        {
            var currentFocus = !token.HasFocus ?
                                token.HasHover && input.IsLeftButtonClicked() :
                                input.IsLeftButtonPressed();

            if (currentFocus && !token.HasFocus)
            {
                entry.Events.OnGotFocus();

                //entry.FocusIndex.Current = entryId;
                token.GotFocusBy = this;
            }
            else if (!currentFocus && token.HasFocus)
            {
                entry.Events.OnLostFocus();
            }
            token.HasFocus = currentFocus;

            token.IsSelected = token.HasFocus && token.HasHover && token.HasGottenFocusBy(this);
        }

        private void CheckClick(AControl entry, AggregationToken token, int entryId)
        {
            if (token.IsSelected && input.IsLeftButtonReleased())
            {
                if (token.HasGottenFocusBy(this))
                {
                    entry.Events.OnClicked();
                }
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

            if (entry.Options.IsFocusEnabled)
            {
                CheckFocus(entry, token, entryId);
            }

            CheckClick(entry, token, entryId);
        }

        #endregion

    }
}
