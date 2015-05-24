using Gem.Gui.Alignment;
using Gem.Gui.Styles;
using Microsoft.Xna.Framework;
using System;
using Gem.Gui.Fluent;
using Gem.Gui.ScreenSystem;
using Microsoft.Xna.Framework.Input;

namespace Gem.Gui.Example
{
    public class NewGameScreen
    {
        private void DisableGui(object sender, EventArgs args)
        {
            if (gui.IsEnabled)
                gui.Disable();
        }
        private readonly GemGui gui;

        public NewGameScreen(GemGui gui, GuiExample game)
        {
            this.gui = gui;
            var playButton =
                gui.Button(0, 0, 100, 100, style: Style.Transparent, pattern: Pattern.SolidColor(Color.White))
                         .Sprite("frame", gui.Textures["frame"])
                         .Color(Color.White)
                         .Text(gui.Fonts["segoe-18"], "Play")
                         .TextColor(Color.Black)
                         .TextHorizontalAlignment(HorizontalAlignment.Center)
                         .TextVerticalAlignment(VerticalAlignment.Center)
                         .ScreenAlignment(HorizontalAlignment.RelativeTo(() => 0.0f),
                                          VerticalAlignment.Center)
                         .OnClick((sender, args) => gui.Hide(GuiScreen.NewGame));

            var backButton =
                       gui.Button(0, 0, 100, 100, Style.Transparent, Pattern.SolidColor(Color.White))
                          .Color(Color.White)
                          .Text(gui.Fonts["segoe-18"], "Back")
                          .TextColor(Color.Black)
                          .TextHorizontalAlignment(HorizontalAlignment.Center)
                          .TextVerticalAlignment(VerticalAlignment.Center)
                          .ScreenAlignment(HorizontalAlignment.Right,
                                          VerticalAlignment.Center)
                          .OnClick((sender, args) =>
                                   gui.Swap(GuiScreen.NewGame, GuiScreen.MainMenu));

            var slider =
                gui.Slider(x: 50, y: 50,
                sizeX: 200, sizeY: 10,
                sliderSizeX: 5, sliderSizeY: 30,
                sliderInfo: new Controls.SliderInfo(0.0f, 100.0f, 1.0f, 0.0f),
                background: Pattern.SolidColor(Color.White),
                slider: Pattern.SolidColor(Color.Black),
                filling: Pattern.SolidColor(Color.Blue),
                border: Pattern.Border(Color.Black,Color.Transparent),
                style: Style.CustomisedTransparent(0.6f, 0.3f, 0.0f))
                .ScreenAlignment(HorizontalAlignment.Center,
                                 VerticalAlignment.Center);

            gui.AddGuiHost(GuiScreen.NewGame, playButton, slider, backButton);

            gui[GuiScreen.NewGame].OnEntering += (sender, args) =>
            {
               // Input.InputManager.KeyboardInputKeys.Previous = Keys.Left;
               // Input.InputManager.KeyboardInputKeys.Next = Keys.Right;
            };

            gui[GuiScreen.NewGame].Transition = new TimedTransition(
                                                      TimeSpan.FromSeconds(0.5),
                                                      (state, progress, target, batch) =>
                                                      batch.Draw(target,
                                                                 Vector2.Zero,
                                                                 Color.White * progress));
        }

    }
}
