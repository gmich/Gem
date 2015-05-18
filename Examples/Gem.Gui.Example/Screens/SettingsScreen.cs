using Gem.Gui.Alignment;
using Gem.Gui.Styles;
using Microsoft.Xna.Framework;
using System;
using Gem.Gui.Fluent;
using Gem.Gui.ScreenSystem;

namespace Gem.Gui.Example
{
    public class SettingsScreen
    {
        public SettingsScreen(GemGui gui, GuiExample game)
        {

            var backButton =
                       gui.Button(0, 0, 100, 100, style: Style.Transparent)
                          .Color(Color.White)
                          .Text(gui.Fonts["segoe-10"], "Back")
                          .TextColor(Color.Black)
                          .TextHorizontalAlignment(HorizontalAlignment.Center)
                          .TextVerticalAlignment(VerticalAlignment.Center)
                          .ScreenAlignment(HorizontalAlignment.Center,
                                          VerticalAlignment.Bottom)
                          .OnClick((sender, args) =>
                                   gui.Swap(GuiScreen.Settings, GuiScreen.MainMenu));

            var textBox = gui.TextField(x: 300, y: 100,
                                      sizeX: 300, sizeY: 30,
                                      textColor: Color.Black,
                                      font: gui.Fonts["segoe-10"],
                                      style: Style.Transparent,
                                      pattern: Pattern.Border,
                                      hint: "Your name")
                                      .OnTextEntry((sender, text) => game.PlayerName = text)
                                      .ScreenAlignment(HorizontalAlignment.Center,
                                                       VerticalAlignment.Center);
            
            gui.AddGuiHost(GuiScreen.Settings, backButton, textBox);

            gui[GuiScreen.Settings].Transition = new TimedTransition(
                                                      TimeSpan.FromSeconds(0.5),
                                                      (state, progress, target, batch) =>
                                                      batch.Draw(target,
                                                                 (float)Math.Pow(progress - 1.0f, 2) * new Vector2(0, 300),
                                                                 Color.White * progress));
        }


    }
}
