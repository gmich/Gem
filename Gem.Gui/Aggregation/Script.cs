using Gem.Gui.Input;

namespace Gem.Gui.Aggregation
{
    /// <summary>
    /// Factory for returning scripts that are used by ScriptAggregator to aggregate the controls 
    /// </summary>
    public static class Script
    {
        public static ScriptAggregator<KeyboardInputHelper> ForKeyboard(KeyboardInputKeys keyboardKeys)
        {
            return new ScriptAggregator<KeyboardInputHelper>(InputManager.Keyboard,
                                                             //never disable
                                                             input => false,
                                                             input => input.IsKeyPressed(keyboardKeys.Next),
                                                             input => input.IsKeyPressed(keyboardKeys.Previous),
                                                             input => input.IsKeyClicked(keyboardKeys.Trigger),
                                                             InputManager.KeyRepetition);
        }

        public static ScriptAggregator<GamePadInputHelper> ForGamePad(GamePadInputButtons gamepadButtons)
        {
            return new ScriptAggregator<GamePadInputHelper>(InputManager.GamePad,
                                                             input => !input.IsConnected,
                                                             input => input.IsButtonPressed(gamepadButtons.Next),
                                                             input => input.IsButtonPressed(gamepadButtons.Previous),
                                                             input => input.IsButtonClicked(gamepadButtons.Trigger),
                                                             InputManager.KeyRepetition);
        }
    }
}
