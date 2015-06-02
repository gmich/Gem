using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using Gem.Infrastructure.Attributes;

namespace Gem.Infrastructure.Input
{
    public class GamePadInput : IInput
    {

        #region Fields

        private GamePadState gamePadState;
        private GamePadState previousGamePadState;

        #endregion

        #region Ctor

        public GamePadInput()
        {
            this.AssignDefaultValues();
            gamePadState = GamePad.GetState(PlayerIndex);
        }

        #endregion

        #region Properties

        [DefaultValue(PlayerIndex.One)]
        public PlayerIndex PlayerIndex
        {
            get;
            set;
        }

        #endregion

        #region Settings

        public bool IsConnected
        {
            get { return gamePadState.IsConnected; }
        }

        #endregion

        #region Buttons

        public bool IsButtonPressed(Buttons buttonToTest)
        {
            return gamePadState.IsButtonDown(buttonToTest);
        }

        public bool IsButtonReleased(Buttons buttonToTest)
        {
            return (gamePadState.IsButtonUp(buttonToTest)
                   && previousGamePadState.IsButtonDown(buttonToTest));
        }

        public bool IsButtonClicked(Buttons buttonToTest)
        {
            return (gamePadState.IsButtonDown(buttonToTest)
                   && previousGamePadState.IsButtonUp(buttonToTest));
        }

        #endregion

        #region Thumpstick

        public Vector2 LeftThumpstick
        {
            get { return gamePadState.ThumbSticks.Left; }
        }

        public Vector2 LeftThumpstickDelta
        {
            get { return gamePadState.ThumbSticks.Left - previousGamePadState.ThumbSticks.Left; }
        }

        public Vector2 RightThumpstick
        {
            get { return gamePadState.ThumbSticks.Right; }
        }

        public Vector2 RightThumpstickDelta
        {
            get { return gamePadState.ThumbSticks.Right - previousGamePadState.ThumbSticks.Right; }
        }

        #endregion

        #region Triggers

        public float LeftTriggerPressure
        {
            get { return gamePadState.Triggers.Left; }
        }

        public float LeftTriggerPressureDelta
        {
            get { return gamePadState.Triggers.Left - gamePadState.Triggers.Left; }
        }

        public float RightTriggerPressure
        {
            get { return gamePadState.Triggers.Right; }
        }

        public float RightTriggerPressureDelta
        {
            get { return gamePadState.Triggers.Right - gamePadState.Triggers.Right; }
        }

        #endregion

        #region Update

        public void Flush()
        {
            previousGamePadState = gamePadState;
            gamePadState = GamePad.GetState(PlayerIndex);
        }
        
        #endregion

    }
}