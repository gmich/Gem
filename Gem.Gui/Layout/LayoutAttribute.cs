using Gem.Gui.Controls;

namespace Gem.Gui.Layout
{
    public class LayoutAttribute : System.Attribute
    {
        private readonly string tag;

        public LayoutAttribute(string tag)
        {
            this.tag = tag;
        }

        public string Tag { get { return tag; } }
    }
}
