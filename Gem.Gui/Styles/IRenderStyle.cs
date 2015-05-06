using Gem.Gui.Controls;

namespace Gem.Gui.Core.Styles
{
    public interface IRenderStyle
    {
        void Focus();
        void Default();
        void Hover();
        void Clicked();
    }
}
