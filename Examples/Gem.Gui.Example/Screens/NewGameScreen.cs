using Gem.Gui.Alignment;
using Gem.Gui.Styles;
using Microsoft.Xna.Framework;
using System;
using Gem.Gui.Fluent;
using Gem.Gui.ScreenSystem;

namespace Gem.Gui.Example
{
    public class NewGameScreen
    {
        public NewGameScreen(GemGui gui, GuiExample game)
        {
            var playButton =
                      gui.Button(0, 0, 100, 100, style: Style.Transparent)
                         .Sprite("frame",gui.Textures["frame"])
                         .Color(Color.White)
                         .Text(gui.Fonts["segoe-10"], "Play")
                         .TextColor(Color.Black)
                         .TextHorizontalAlignment(HorizontalAlignment.Center)
                         .TextVerticalAlignment(VerticalAlignment.Center)
                         .ScreenAlignment(HorizontalAlignment.RelativeTo(() => 0.0f, 0),
                                          VerticalAlignment.Center)
                         .OnClick((sender, args) =>
                                  gui.Swap(GuiScreen.MainMenu, GuiScreen.NewGame));

            var backButton =
                       gui.Button(0, 0, 100, 100, style: Style.Transparent)
                          .Color(Color.White)
                          .Text(gui.Fonts["segoe-10"], "Back")
                          .TextColor(Color.Black)
                          .TextHorizontalAlignment(HorizontalAlignment.Center)
                          .TextVerticalAlignment(VerticalAlignment.Center)
                         .ScreenAlignment(HorizontalAlignment.Right,
                                          VerticalAlignment.Center)
                          .OnClick((sender, args) =>
                                   gui.Swap(GuiScreen.NewGame, GuiScreen.MainMenu));
            

            gui.AddGuiHost(GuiScreen.NewGame, playButton,backButton);

            gui[GuiScreen.NewGame].Transition = new TimedTransition(
                                                      TimeSpan.FromSeconds(0.5),
                                                      (state, progress, target, batch) =>
                                                      batch.Draw(target,
                                                                 Vector2.Zero,
                                                                 Color.White * progress));
        }

    }
}
