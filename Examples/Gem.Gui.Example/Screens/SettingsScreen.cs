using Gem.Gui.Alignment;
using Gem.Gui.Styles;
using Microsoft.Xna.Framework;
using System;
using Gem.Gui.Fluent;
using Gem.Gui.ScreenSystem;
using System.Linq;

namespace Gem.Gui.Example
{
    public class SettingsScreen
    {
        public SettingsScreen(GemGui gui, GuiExample game)
        {
            Color textColor = new Color(137, 161, 161);
            Color colorMap = new Color(200, 161, 161);
            int entryOffset = 10;

            var backButton =
                gui.Button(0, 0, 100, 50,
                           Style.ColorMap(textColor, colorMap),
                           Pattern.SolidColor(colorMap))
                          .Text(gui.Fonts["segoe-18"], "Back")
                          .TextColor(new Color(200, 200, 200))
                          .TextHorizontalAlignment(HorizontalAlignment.Center)
                          .TextVerticalAlignment(VerticalAlignment.Center)
                          .ScreenAlignment(HorizontalAlignment.Center,
                                          VerticalAlignment.Bottom)
                          .OnClick((sender, args) =>
                                   gui.Swap(GuiScreen.Settings, GuiScreen.MainMenu));

            var nameTextBox = gui.TextField(x: 300, y: 100,
                                      sizeX: 300, sizeY: 30,
                                      textColor: Color.Black,
                                      font: gui.Fonts["segoe-18"],
                                      style: Style.Transparent,
                                      pattern: Pattern.TextViewPattern(Color.Black, Color.White),
                                      hint: "Your name")
                                      .OnTextEntry((sender, text) => game.PlayerName = text)
                                      .ScreenAlignment(HorizontalAlignment.Center,
                                                       VerticalAlignment.Center);

            var emailTextBox = gui.TextField(x: 300, y: 100,
                                             sizeX: 300, sizeY: 30,
                                             textColor: Color.Black,
                                             font: gui.Fonts["segoe-18"],
                                             style: Style.Transparent,
                                             pattern: Pattern.TextViewPattern(Color.Black, Color.White),
                                             hint: "Your email");

            var checkBox =
                        gui.CheckBox(10, 10, 140, 30,
                                    Pattern.SolidColor(textColor),
                                    20, 20,
                                    Pattern.Border(Color.DarkGreen, textColor),
                                    Pattern.Border(Color.DarkGreen, Color.Transparent),
                                    Style.CustomisedTransparent(0.6f, 0.3f, 0.0f),
                                    "CheckBox",
                                    gui.Fonts["segoe-18"])
                                    .OnCheckChanged((sender, value) => Console.WriteLine("Checkbox new value is {0}", value))
                                    .TextColor(Color.Black);

            var slider =
                gui.Slider(x: 50, y: 50,
                           sizeX: 200, sizeY: 20,
                           sliderSizeX: 15, sliderSizeY: 25,
                           sliderInfo: new Controls.SliderInfo(minValue: 10.0f, maxValue: 170.0f, step: 1.0f, initialPosition: 10.0f),
                           background: Pattern.SolidColor(Color.White),
                           slider: Pattern.SolidColor(Color.Black),
                           filling: Pattern.SolidColor(new Color(0, 0, 0, 100)),
                           border: Pattern.Border(Color.Black, Color.Transparent),
                           style: Style.CustomisedTransparent(0.6f, 0.3f, 0.0f));

            var sliderLabel =
                gui.Label(x: 10, y: 10,
                          sizeX: 30, sizeY: 30,
                          text: slider.SliderValue.ToString(),
                          font: gui.Fonts["segoe-18"],
                          textColor: Color.White,
                          horizontalAlignment: HorizontalAlignment.Center,
                          verticalAlignment: VerticalAlignment.Center,
                          pattern: Pattern.SolidColor(Color.Black))
                          .StretchToText(true);

            var label = 
                gui.Label(x: 300, y: 100,
                          sizeX: 300, sizeY: 30,
                          text: "Profile",
                          font: gui.Fonts["segoe-18"],
                          textColor: Color.White,
                          horizontalAlignment: HorizontalAlignment.Center,
                          verticalAlignment: VerticalAlignment.Center,
                          pattern: Pattern.SolidColor(Color.Transparent));

            slider.OnValueChange += (sender, value) => sliderLabel.Text.Value = value.ToString();
            slider.ScreenAlignment(HorizontalAlignment.Center,
                                  slider.VerticalRelativeTo(checkBox, () => checkBox.Region.Position.Y
                                                                                  + checkBox.Region.Size.Y
                                                                                  + entryOffset * Configuration.Settings.Scale.Y));

            sliderLabel.ScreenAlignment(sliderLabel.HorizontalRelativeTo(slider, () => slider.Region.Frame.Right + 5 * Configuration.Settings.Scale.X),
                                  sliderLabel.VerticalRelativeTo(slider, () => slider.Region.Position.Y));
            
            emailTextBox.ScreenAlignment(HorizontalAlignment.Center,
                                 emailTextBox.VerticalRelativeTo(nameTextBox, () => nameTextBox.Region.Position.Y
                                                                                  + nameTextBox.Region.Size.Y
                                                                                  + entryOffset * Configuration.Settings.Scale.Y));

            checkBox.ScreenAlignment(checkBox.HorizontalRelativeTo(emailTextBox, () => emailTextBox.Region.Position.X),
                     checkBox.VerticalRelativeTo(emailTextBox, () => emailTextBox.Region.Position.Y
                                                                    + emailTextBox.Region.Size.Y
                                                                    + entryOffset * Configuration.Settings.Scale.Y));



            label.ScreenAlignment(HorizontalAlignment.Center,
                                  label.VerticalRelativeTo(nameTextBox, () => nameTextBox.Region.Position.Y - label.Region.Size.Y));

            gui.AddGuiHost(GuiScreen.Settings, nameTextBox, emailTextBox, checkBox,slider, backButton, label, sliderLabel);

            gui[GuiScreen.Settings].Transition = new TimedTransition(
                                                      TimeSpan.FromSeconds(0.5),
                                                      (state, progress, target, batch) =>
                                                      batch.Draw(target,
                                                                 (float)Math.Pow(progress - 1.0f, 2) * new Vector2(0, 300),
                                                                 Color.White * progress));
        }

    }
}
