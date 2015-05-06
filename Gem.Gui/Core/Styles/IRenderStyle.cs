using Gem.Gui.Controls;

namespace Gem.Gui.Core.Styles
{
    public interface IRenderStyle
    {
        void Focus(AControl control);
        void Default(AControl control);
        void Hover(AControl control);
        void Clicked(AControl control);
    }
}
