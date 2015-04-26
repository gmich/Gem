using Gem.Gui.Elements.Areas;
using System;

namespace Gem.Gui.Aggregation
{
    public class AggregationEnumerator : IAggregator
    {
        #region Fields

        private readonly Func<bool> next;
        private readonly Func<bool> previous;
        private readonly Func<bool> trigger;

        #endregion

        public AggregationEnumerator(Func<bool> next, Func<bool> previous, Func<bool> trigger)
        {
            this.next = next;
            this.previous = previous;
        }

        private bool _isActive = true;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public void Aggregate(IGuiArea area, AggregationContext context)
        {
            if (next())
            {
                area[area.FocusIndex.Current].Events.OnLostFocus();
                area[area.FocusIndex.Next].Events.OnGotFocus();
            }
            if (previous())
            {
                area[area.FocusIndex.Current].Events.OnLostFocus();
                area[area.FocusIndex.Previous].Events.OnGotFocus();
            }
            if (trigger())
            {
                area[area.FocusIndex.Current].Events.OnClicked();
            }
        }
    }
}
