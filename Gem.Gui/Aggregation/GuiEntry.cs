using Gem.Gui.Controls;

namespace Gem.Gui.Aggregation
{
    public class GuiEntry
    {
        private readonly AControl control;

        public GuiEntry(AControl control)
        {
            this.control = control;
            Token = new AggregationToken();
        }

        public AControl Control { get { return control; } }

        public AggregationToken Token { get; set; }
    }
}
