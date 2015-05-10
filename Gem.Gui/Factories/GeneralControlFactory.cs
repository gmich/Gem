using Gem.Gui.Alignment;
using Gem.Gui.Controls;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.ObjectModel;

namespace Gem.Gui.Factories
{
    public class GeneralControlFactory : IControlFactory
    {
        public ImageButton CreateImageButton()
        {
            throw new NotImplementedException();
        }

        public Label CreateLabel()
        {
            throw new NotImplementedException();
        }

        public TextField CreateTextBox()
        {
            throw new NotImplementedException();
        }

        public Button CreateButton(Region region, Texture2D texture)
        {
            return new Button(texture, region);
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
