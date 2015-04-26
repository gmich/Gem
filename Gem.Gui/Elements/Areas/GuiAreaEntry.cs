using Gem.Gui.Controls;

namespace Gem.Gui.Elements.Areas
{
    public class GuiAreaEntry
    {
        private readonly IGuiComponent component;

        public GuiAreaEntry(IGuiComponent component)
        {
            this.component = component;
            Token = new AggregationToken();
        }

        public IGuiComponent Component { get { return component; } }

        public AggregationToken Token { get; set; }
    }
}
