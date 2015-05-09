using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Gui.Factories
{
    //TODO: update method parameters
    public interface IControlFactory
    {
        Button CreateButton(Region region,  Func<Region, Vector2> originCalculator, Texture2D texture);
        ImageButton CreateImageButton();
        Label CreateLabel();
        TextField CreateTextBox();
    }
}
