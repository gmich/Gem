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

            var styleTextBox = gui.TextField(x: 300, y: 100,
                                             sizeX: 300, sizeY: 30,
                                             textColor: Color.Black,
                                             font: gui.Fonts["segoe-18"],
                                             style: Style.Transparent,
                                             pattern: Pattern.TextViewPattern(Color.Black, Color.White),
                                             hint: "Your style");

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


            styleTextBox.ScreenAlignment(HorizontalAlignment.Center,
                                 styleTextBox.VerticalRelativeTo(nameTextBox, () => nameTextBox.Region.Position.Y
                                                                                  + nameTextBox.Region.Size.Y
                                                                                  + entryOffset * Configuration.Settings.Scale.Y));

            checkBox.ScreenAlignment(checkBox.HorizontalRelativeTo(styleTextBox, () => styleTextBox.Region.Position.X),
                     checkBox.VerticalRelativeTo(styleTextBox, () => styleTextBox.Region.Position.Y
                                                                      + styleTextBox.Region.Size.Y
                                                                      + entryOffset * Configuration.Settings.Scale.Y));

            var label = gui.Label(x: 300, y: 100,
                          sizeX: 300, sizeY: 30,
                          text: "Profile",
                          font: gui.Fonts["segoe-18"],
                          textColor: Color.White,
                          horizontalAlignment: HorizontalAlignment.Center,
                          verticalAlignment: VerticalAlignment.Center,
                          pattern: Pattern.SolidColor(Color.Transparent));

            label.ScreenAlignment(HorizontalAlignment.Center,
                                  label.VerticalRelativeTo(nameTextBox, () => nameTextBox.Region.Position.Y - label.Region.Size.Y));

            gui.AddGuiHost(GuiScreen.Settings, nameTextBox, styleTextBox, checkBox, backButton, label);

            gui[GuiScreen.Settings].Transition = new TimedTransition(
                                                      TimeSpan.FromSeconds(0.5),
                                                      (state, progress, target, batch) =>
                                                      batch.Draw(target,
                                                                 (float)Math.Pow(progress - 1.0f, 2) * new Vector2(0, 300),
                                                                 Color.White * progress));
        }

    }
}
