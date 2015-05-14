using Gem.Gui.Alignment;
using Gem.Gui.Controls;
using Gem.Gui.Styles;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.ObjectModel;

namespace Gem.Gui.Factories
{
    //TODO: update method parameters
    public interface IControlFactory
    {
        Button CreateButton(Region region, Texture2D texture, ARenderStyle style);

        ListView CreateListView(Texture2D texture,
                                Region region,
                                Orientation orientation,
                                AlignmentContext alignmentContext,
                                ReadOnlyCollection<AControl> controls);

        TextField CreateTextBox();
    }
}
