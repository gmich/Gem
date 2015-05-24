using Gem.Gui.Alignment;
using Gem.Gui.Controls;
using Gem.Gui.Styles;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace Gem.Gui.Factories
{
    public class GeneralControlFactory : IControlFactory
    {

        public Button CreateButton(Region region, Texture2D texture, ARenderStyle style)
        {
            return new Button(texture, region, style);
        }

        public ListView CreateListView(Texture2D texture,
                                       Region region,
                                       Orientation orientation,
                                       AlignmentContext alignment,
                                       ReadOnlyCollection<AControl> controls)
        {
            return new ListView(texture,
                                alignment,
                                orientation,
                                region,
                                controls);
        }

        public TextField CreateTextBox(TextAppenderHelper appender,
                                       SpriteFont font,
                                       Texture2D texture,
                                       Region region,
                                       Color textcolor,
                                       ARenderStyle style,
                                       string hint,
                                       IHorizontalAlignable horizontalAlignment,
                                       IVerticalAlignable verticalAlignment,
                                       IAlignmentTransition transition)
        {
            return new TextField(appender,
                                 font,
                                 texture,
                                 region,
                                 textcolor,
                                 style,
                                 hint,
                                 new AlignmentContext(horizontalAlignment, verticalAlignment, transition));
        }




        public Label CreateLabel(string text,
                                     SpriteFont font,
                                     Color textcolor,
                                     Texture2D texture,
                                     Region region,
                                     IHorizontalAlignable horizontalAlignment,
                                     IVerticalAlignable verticalAlignment,
                                     IAlignmentTransition transition)
        {
            return new Label(text,
                            font,
                            texture,
                            region,
                            textcolor,
                            new AlignmentContext(horizontalAlignment, verticalAlignment, transition));
        }


        public CheckBox CreateCheckBox(Texture2D backgroundTexture,
                                       Texture2D checkedTexture, 
                                       Texture2D unCheckedTexture, 
                                       ARenderStyle style, 
                                       Region region,
                                       IHorizontalAlignable checkBoxHorizontalAlignment, 
                                       IVerticalAlignable checkBoxVerticalAlignment,
                                       string text,
                                       SpriteFont font)
        {
            return new CheckBox(backgroundTexture,
                                region,
                                style,
                                new AlignmentContext(checkBoxHorizontalAlignment, checkBoxVerticalAlignment, AlignmentTransition.Fixed), 
                                checkedTexture, 
                                unCheckedTexture,
                                text,
                                font);
        }


        public Slider CreateSlider(SliderInfo sliderInfo,
                                   Texture2D backgroundTexture, 
                                   Texture2D slider, 
                                   Texture2D filling,
                                   Texture2D border, 
                                   Region region,
                                   Region borderRegion,
                                   Region sliderRegion, 
                                   ARenderStyle style)
        {
            return new Slider(sliderInfo, 
                              new SliderDrawable(slider,
                                                 border,
                                                 filling, 
                                                 borderRegion,
                                                 new AlignmentContext(HorizontalAlignment.Left,
                                                                      VerticalAlignment.Center,
                                                                      AlignmentTransition.Fixed)), 
                                                 backgroundTexture,
                                                 region,
                                                 style);
        }
    }
}
