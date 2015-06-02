using Gem.Gui.Input;
using Gem.Infrastructure.Input;

namespace Gem.Gui.Aggregation
{
    /// <summary>
    /// Factory for returning scripts that are used by ScriptAggregator to aggregate the controls 
    /// </summary>
    public static class Script
    {
        public static ScriptAggregator<KeyboardInput> ForKeyboard(KeyboardInputKeys keyboardKeys)
        {
            return new ScriptAggregator<KeyboardInput>(InputManager.Keyboard,
                                                             //never disable
                                                             input => false,
                                                             input => input.IsKeyPressed(keyboardKeys.Next),
                                                             input => input.IsKeyPressed(keyboardKeys.Previous),
                                                             input => input.IsKeyClicked(keyboardKeys.Trigger),
                                                             InputManager.KeyRepetition);
        }

        public static ScriptAggregator<GamePadInput> ForGamePad(GamePadInputButtons gamepadButtons)
        {
            return new ScriptAggregator<GamePadInput>(InputManager.GamePad,
                                                             input => !input.IsConnected,
                                                             input => input.IsButtonPressed(gamepadButtons.Next),
                                                             input => input.IsButtonPressed(gamepadButtons.Previous),
                                                             input => input.IsButtonClicked(gamepadButtons.Trigger),
                                                             InputManager.KeyRepetition);
        }
    }
}
