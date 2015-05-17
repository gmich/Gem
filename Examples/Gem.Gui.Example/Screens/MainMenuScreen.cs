using Gem.Gui.Alignment;
using Gem.Gui.Styles;
using Microsoft.Xna.Framework;
using System;
using Gem.Gui.Fluent;
using Gem.Gui.ScreenSystem;
using Microsoft.Xna.Framework.Input;

namespace Gem.Gui.Example
{
    public class MainMenuScreen
    {
        public MainMenuScreen(GemGui gui, GuiExample game)
        {
            var newGameButton =
                      gui.Button(0, 0, 130, 100, style: Style.Transparent)
                         .Color(Color.White)
                         .Text(gui.Fonts["segoe-10"], "New Game")
                         .TextColor(Color.Black)
                         .TextHorizontalAlignment(HorizontalAlignment.Center)
                         .TextVerticalAlignment(VerticalAlignment.Center)
                         .Padding(top: 40, bottom: 10)
                         .OnClick((sender, args) =>
                                  gui.Swap(GuiScreen.MainMenu, GuiScreen.NewGame));

            var settingsButton =
                       gui.Button(0, 0, 130, 100, style: Style.Transparent)
                          .Color(Color.White)
                          .Text(gui.Fonts["segoe-10"], "Settings")
                          .TextColor(Color.Black)
                          .TextHorizontalAlignment(HorizontalAlignment.Center)
                          .TextVerticalAlignment(VerticalAlignment.Center)
                          .Padding(top: 10, bottom: 10)
                          .OnClick((sender, args) =>
                                   gui.Swap(GuiScreen.MainMenu, GuiScreen.Settings));

            var exitButton =
                       gui.Button(0, 0, 130, 100, style: Style.Transparent)
                          .Color(Color.White)
                          .Text(gui.Fonts["segoe-10"], "Quit")
                          .TextColor(Color.Black)
                          .TextHorizontalAlignment(HorizontalAlignment.Center)
                          .TextVerticalAlignment(VerticalAlignment.Center)
                          .Padding(top: 10, bottom: 10)
                          .OnClick((sender, args) => game.Exit());

            var listView =
                      gui.ListView(x: 0, y: 0,
                                  sizeX: 200, sizeY: 400,
                                  orientation: Layout.Orientation.Portrait,
                                  horizontalAlignment: HorizontalAlignment.Center,
                                  verticalAlignment: VerticalAlignment.Center,
                                  alignmentTransition: AlignmentTransition.Fixed,
                                  controls: new[] { newGameButton, settingsButton, exitButton })
                          .ScreenAlignment(HorizontalAlignment.Center,
                                           VerticalAlignment.Center)
                          .Color(Color.Black)
                          .Text(gui.Fonts["segoe-10"], "Main Menu")
                          .TextColor(Color.White)
                          .TextHorizontalAlignment(HorizontalAlignment.Center)
                          .TextVerticalAlignment(VerticalAlignment.Top);

            gui.AddGuiHost(GuiScreen.MainMenu, listView);
            
            gui[GuiScreen.MainMenu].OnEnter += (sender, args) =>
            {
                Input.InputManager.KeyboardMenuScript.Previous = Keys.Up;
                Input.InputManager.KeyboardMenuScript.Next = Keys.Down;
            };

            gui[GuiScreen.MainMenu].Transition = new TimedTransition(
                                                      TimeSpan.FromSeconds(0.5),
                                                      (state, progress, target, batch) =>
                                                      batch.Draw(target,
                                                                 (float)Math.Pow(progress - 1.0f, 2) * new Vector2(300, 0),
                                                                 Color.White * progress));
        }

    }
}
