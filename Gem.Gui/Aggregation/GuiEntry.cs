using Gem.Gui.Controls;

namespace Gem.Gui.Aggregation
{
    public class GuiEntry
    {
        private readonly AControl control;
        private readonly int index;

        public GuiEntry(AControl control, int index)
        {
            this.control = control;
            this.index = index;
            Token = new AggregationToken();
        }

        public int Index { get { return index; } }

        public AControl Control { get { return control; } }

        public AggregationToken Token { get; set; }
    }
}
