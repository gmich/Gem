using Gem.Gui.Alignment;
using Gem.Gui.Styles;
using Microsoft.Xna.Framework;
using System;
using Gem.Gui.Fluent;
using Gem.Gui.ScreenSystem;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using Gem.Gui.Transformations;

namespace Gem.Gui.Example
{
    public class MainMenuScreen
    {
        public MainMenuScreen(GemGui gui, GuiExample game)
        {
            Color textColor = new Color(137,161,161);
            Color colorMap = new Color(200,161,161);

            var newGameButton =
                gui.Button(0, 0, 180, 80, 
                           Style.ColorMap(textColor, colorMap),
                           Pattern.SolidColor(colorMap))
                         .Text(gui.Fonts["segoe-18"], "New Game")
                         .TextColor(textColor)
                         .TextHorizontalAlignment(HorizontalAlignment.Center)
                         .TextVerticalAlignment(VerticalAlignment.Center)
                         .Padding(top: 20, bottom: 10)
                         .OnClick((sender, args) =>
                                  gui.Swap(GuiScreen.MainMenu, GuiScreen.NewGame));

            var settingsButton =
                       gui.Button(0, 0, 180, 80, Style.ColorMap(textColor, colorMap), pattern: Pattern.SolidColor(colorMap))
                          .Text(gui.Fonts["segoe-18"], "Settings")
                          .TextColor(textColor)
                          .TextHorizontalAlignment(HorizontalAlignment.Center)
                          .TextVerticalAlignment(VerticalAlignment.Center)
                          .Padding(top: 10, bottom: 10)
                          .OnClick((sender, args) =>
                                   gui.Swap(GuiScreen.MainMenu, GuiScreen.Settings));

            var exitButton =
                       gui.Button(0, 0, 180, 80, style: Style.ColorMap(textColor, colorMap), pattern: Pattern.SolidColor(colorMap))
                          .Text(gui.Fonts["segoe-18"], "Quit")
                          .TextColor(textColor)
                          .TextHorizontalAlignment(HorizontalAlignment.Center)
                          .TextVerticalAlignment(VerticalAlignment.Center)
                          .Padding(top: 10, bottom: 10)
                          .OnClick((sender, args) => game.Exit());

            var listView =
                      gui.ListView(x: 0, y: 0,
                                  sizeX: 200, sizeY: 300,
                                  orientation: Layout.Orientation.Portrait,
                                  horizontalAlignment: HorizontalAlignment.Center,
                                  verticalAlignment: VerticalAlignment.Center,
                                  alignmentTransition: AlignmentTransition.Fixed,
                                  controls: new[] { newGameButton, settingsButton, exitButton })
                          .ScreenAlignment(HorizontalAlignment.Center,
                                           VerticalAlignment.Center)
                          .Color(Color.Transparent);

            gui.AddGuiHost(GuiScreen.MainMenu, listView);
            
            gui[GuiScreen.MainMenu].OnEntering += (sender, args) =>
            {
                Input.InputManager.KeyboardInputKeys.Previous = Keys.Up;
                Input.InputManager.KeyboardInputKeys.Next = Keys.Down;
            };

            gui[GuiScreen.MainMenu].Transition = new TimedTransition(
                                                      TimeSpan.FromSeconds(0.5),
                                                      (state, progress, target, batch) =>
                                                      batch.Draw(target,
                                                                 (float)Math.Pow(progress - 1.0f, 2) * new Vector2(300, 0) * Configuration.Settings.Scale,
                                                                 Color.White * progress));
        }

    }
}
