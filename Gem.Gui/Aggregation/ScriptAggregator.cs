using System;

namespace Gem.Gui.Aggregation
{
    public class ScriptAggregator : IAggregator
    {
        #region Fields

        private readonly Func<bool> next;
        private readonly Func<bool> previous;
        private readonly Func<bool> trigger;

        #endregion

        public ScriptAggregator(Func<bool> next, Func<bool> previous, Func<bool> trigger)
        {
            this.next = next;
            this.previous = previous;
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        public void Aggregate(GuiEntry entry, AggregationContext context)
        {
            if (next())
            {
                //area[area.FocusIndex.Current].Events.OnLostFocus();
                //area[area.FocusIndex.Next].Events.OnGotFocus();
            }
            if (previous())
            {
                //area[area.FocusIndex.Current].Events.OnLostFocus();
                //area[area.FocusIndex.Previous].Events.OnGotFocus();
            }
            if (trigger())
            {
                //area[area.FocusIndex.Current].Events.OnClicked();
            }
        }
    }
}
