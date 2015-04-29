using System;
using Gem.Gui.Elements;
using Gem.Gui.Input;
using Gem.Gui.Controls;
using Microsoft.Xna.Framework;
using Gem.Gui.Elements.Areas;

namespace Gem.Gui.Aggregation
{
    internal class MouseControlAggregator : IAggregator
    {

        #region  Fields

        private readonly Func<IGuiComponent, ElementEventArgs> eventArgsProvider;
        private readonly MouseInputHelper input;

        //this is used to spot new mouse locations
        private Point mouseLocation;

        #endregion
        
        public MouseControlAggregator(MouseInputHelper inputHelper)
        {
            this.input = inputHelper;
        }

        private bool _isActive = true;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                mouseLocation = input.MousePosition;
            }
        }
        #region Event Raising

        private void CheckMouseHover(IGuiArea area, AggregationToken token, int entryId)
        {
            var currentHasHover = area[entryId].Region.Frame.Contains(input.MousePosition);

            if (currentHasHover && !token.HasHover)
            {
                area[entryId].Events.OnMouseCapture();
            }
            else if (!currentHasHover && token.HasHover)
            {
                area[entryId].Events.OnLostMouseCapture();
            }

            token.HasHover = currentHasHover;
        }

        private void CheckFocus(IGuiArea area, AggregationToken token, int entryId)
        {
            var currentFocus = !token.HasFocus ?
                                token.HasHover && input.IsLeftButtonClicked() :
                                input.IsLeftButtonPressed();

            if (currentFocus && !token.HasFocus)
            {
                area[entryId].Events.OnGotFocus();
                area.FocusIndex.Current = entryId;
                token.GotFocusBy = this;
            }
            else if (!currentFocus && token.HasFocus)
            {
                area[entryId].Events.OnLostFocus();
            }
            token.HasFocus = currentFocus;

            token.IsSelected = token.HasFocus && token.HasHover && token.HasGottenFocusBy(this);
        }

        private void CheckClick(IGuiArea area, AggregationToken token, int entryId)
        {
            if (token.IsSelected && input.IsLeftButtonReleased())
            {
                area[entryId].Events.OnClicked();
            }
        }

        #endregion

        #region Aggregation

        public void Aggregate(IGuiArea area, AggregationContext context)
        {
            if (!this.IsActive)
            {
                //if the mouse has moved, then the aggregator is activated
                IsActive = (mouseLocation != input.MousePosition);
            }

            //TODO: add aggregation context to the area
            //Aggregate(area);

            int entryId = 0;
            foreach (var entry in area.Entries())
            {
                Aggregate(area, entry.Token, entryId);
                entryId++;
            }
        }

        private void Aggregate(IGuiArea area, AggregationToken token,int entryId)
        {
            if (!area[entryId].Options.IsEnabled || !this.IsActive)
            {
                return;
            }

            CheckMouseHover(area,token, entryId);

            if (area[entryId].Options.IsFocusEnabled)
            {
                CheckFocus(area, token, entryId);
            }

            CheckClick(area, token, entryId);
        }

        #endregion

    }
}
