using Gem.Gui.Alignment;
using Gem.Gui.Styles;
using Microsoft.Xna.Framework;
using System;
using Gem.Gui.Fluent;
using Gem.Gui.ScreenSystem;
using Microsoft.Xna.Framework.Input;
using Gem.Gui.Controls;

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
                sizeX: 200, sizeY: 20,
                sliderSizeX: 15, sliderSizeY: 25,
                sliderInfo: new Controls.SliderInfo(minValue: 10.0f, maxValue: 170.0f, step: 1.0f, initialPosition: 10.0f),
                background: Pattern.SolidColor(Color.White),
                slider: Pattern.SolidColor(Color.Black),
                filling: Pattern.SolidColor(new Color(0, 0, 0, 100)),
                border: Pattern.Border(Color.Black, Color.Transparent),
                style: Style.CustomisedTransparent(0.6f, 0.3f, 0.0f))
                .ValueChanged((sender, value) => Console.WriteLine("slider: " + value));

            var largeStepSlider =
                gui.Slider(x: 50, y: 50,
                sizeX: 300, sizeY: 20,
                sliderSizeX: 15, sliderSizeY: 25,
                sliderInfo: new Controls.SliderInfo(minValue: 0.0f, maxValue: 300.0f, step: 20.0f, initialPosition: 300.0f),
                background: Pattern.SolidColor(Color.White),
                slider: Pattern.SolidColor(Color.Black),
                filling: Pattern.SolidColor(new Color(0, 0, 0, 100)),
                border: Pattern.Border(Color.Black, Color.Transparent),
                style: Style.CustomisedTransparent(0.6f, 0.3f, 0.0f))
                .ValueChanged((sender, value) => Console.WriteLine("LargeStepSlider: " + value))
               .ScreenAlignment(HorizontalAlignment.Center,
                                   VerticalAlignment.Bottom);
            var tinyStepSlider =
                gui.Slider(x: 50, y: 50,
                sizeX: 200, sizeY: 20,
                sliderSizeX: 15, sliderSizeY: 25,
                sliderInfo: new Controls.SliderInfo(minValue: 10.0f, maxValue: 170.0f, step: 0.01f, initialPosition: 10.0f),
                background: Pattern.SolidColor(Color.White),
                slider: Pattern.SolidColor(Color.Black),
                filling: Pattern.SolidColor(new Color(0, 0, 0, 100)),
                border: Pattern.Border(Color.Black, Color.Transparent),
                style: Style.CustomisedTransparent(0.6f, 0.3f, 0.0f))
               .ValueChanged((sender, value) => Console.WriteLine("tinyStepSlider: " + value))
               .ScreenAlignment(HorizontalAlignment.Center,
                                   VerticalAlignment.Top);

            var label = gui.Label(x: 10, y: 10,
              sizeX: 30, sizeY: 30,
              text: slider.SliderValue.ToString(),
              font: gui.Fonts["segoe-18"],
              textColor: Color.White,
              horizontalAlignment: HorizontalAlignment.Center,
              verticalAlignment: VerticalAlignment.Center,
              pattern: Pattern.SolidColor(Color.Black));
            label.StretchToText = true;

            slider.OnValueChanging += (sender, value) => label.Text.Value = value.ToString();
            slider.ScreenAlignment(HorizontalAlignment.Center,
                                   VerticalAlignment.Center);

            label.ScreenAlignment(label.HorizontalRelativeTo(slider, () => slider.Region.Frame.Right + 5 * Configuration.Settings.Scale.X),
                                  VerticalAlignment.Center);

            gui.AddGuiHost(GuiScreen.NewGame, playButton, largeStepSlider, slider, tinyStepSlider, backButton, label);

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
