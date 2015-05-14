using Gem.Gui.Alignment;
using Gem.Gui.Controls;
using Gem.Gui.Styles;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.ObjectModel;

namespace Gem.Gui.Factories
{
    public class GeneralControlFactory : IControlFactory
    {

        public TextField CreateTextBox()
        {
            throw new NotImplementedException();
        }

        public Button CreateButton(Region region, Texture2D texture, ARenderStyle style)
        {
            return new Button(texture, region,style);
        }

        public ListView CreateListView(Texture2D texture,
                                       Region region,
                                       Orientation orientation, 
                                       AlignmentContext alignment,
                                       ReadOnlyCollection<AControl> controls)
        {
            return new ListView(texture, alignment, orientation, region, controls);
        }

    }
}
