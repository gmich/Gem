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
    public interface IControlFactory
    {
        Button CreateButton(Region region, Texture2D texture, ARenderStyle style);

        ListView CreateListView(Texture2D texture,
                                Region region,
                                Orientation orientation,
                                AlignmentContext alignmentContext,
                                ReadOnlyCollection<AControl> controls);

        TextField CreateTextBox(TextAppenderHelper appender,
                                SpriteFont font,
                                Texture2D texture,
                                Region region,
                                Color textcolor,
                                ARenderStyle style,
                                string hint,
                                IHorizontalAlignable horizontalAlignment,
                                IVerticalAlignable verticalAlignment,
                                IAlignmentTransition transition);

        Label CreateLabel(string text,
                              SpriteFont font,
                              Color textcolor,
                              Texture2D texture,
                              Region region,
                              IHorizontalAlignable horizontalAlignment,
                              IVerticalAlignable verticalAlignment,
                              IAlignmentTransition transition);

        CheckBox CreateCheckBox(Texture2D backgroundTexture,
                             Texture2D checkedTexture,
                             Texture2D unCheckedTexture,
                             ARenderStyle style,
                             Region region,
                             IHorizontalAlignable checkBoxHorizontalAlignment,
                             IVerticalAlignable checkBoxVerticalAlignment,
                             string text,
                             SpriteFont font);
    }
}
