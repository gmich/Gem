using Gem.Gui.Controls;

namespace Gem.Gui.Factories
{
    //TODO: update method parameters
    public interface IControlFactory
    {
        ImageButton CreateImageButton();
        Button CreateButton();
        Label CreateLabel();
        TextField CreateTextBox();
    }
}
