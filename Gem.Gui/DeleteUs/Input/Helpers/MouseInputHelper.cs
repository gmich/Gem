using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Gem.Gui.Input
{
    public class MouseInputHelper : IInputHelper
    {

        #region Fields

        private MouseState mouseState = Mouse.GetState();
        private MouseState previousMouseState;

        private float scrollWheelValue = Mouse.GetState().ScrollWheelValue;
        private float previousScrollWheelValue;

        #endregion

        #region Mouse

        public Point MousePosition
        {
            get
            {
                return mouseState.Position;
            }
        }

        #region Scroll Wheel

        public bool IsWheelMovingUp()
        {
            return (scrollWheelValue < previousScrollWheelValue);
        }

        public bool IsWheelMovingUp(int sensitivity)
        {
            return ((scrollWheelValue / sensitivity - previousScrollWheelValue / sensitivity) > 0);
        }

        public bool IsWheelMovingDown()
        {
            return (scrollWheelValue > previousScrollWheelValue);
        }

        public bool IsWheelMovingDown(int sensitivity)
        {
            return ((scrollWheelValue / sensitivity - previousScrollWheelValue / sensitivity) < 0);
        }

        #endregion

        #region Pressed

        public bool IsMiddleButtonPressed()
        {
            return (mouseState.MiddleButton == ButtonState.Pressed);
        }

        public bool IsRightButtonPressed()
        {
            return (mouseState.RightButton == ButtonState.Pressed);
        }

        public bool IsLeftButtonPressed()
        {
            return (mouseState.LeftButton == ButtonState.Pressed);
        }

        public bool IsBackButtonPressed()
        {
            return (mouseState.XButton1 == ButtonState.Pressed);
        }

        public bool IsForwardButtonPressed()
        {
            return (mouseState.XButton2 == ButtonState.Pressed);
        }

        public bool IsButtonPressed(ButtonState current)
        {
            return (current == ButtonState.Pressed);
        }

        #endregion

        #region Clicked

        public bool IsMiddleButtonClicked()
        {
            return (mouseState.MiddleButton == ButtonState.Pressed
                   && previousMouseState.MiddleButton == ButtonState.Released);
        }

        public bool IsRightButtonClicked()
        {
            return (mouseState.RightButton == ButtonState.Pressed
                   && previousMouseState.RightButton == ButtonState.Released);
        }

        public bool IsLeftButtonClicked()
        {
            return (mouseState.LeftButton == ButtonState.Pressed
                   && previousMouseState.LeftButton == ButtonState.Released);
        }

        public bool IsBackButtonClicked()
        {
            return (mouseState.XButton1 == ButtonState.Pressed
                   && previousMouseState.XButton1 == ButtonState.Released);
        }

        public bool IsForwardButtonClicked()
        {
            return (mouseState.XButton2 == ButtonState.Pressed
                   && previousMouseState.XButton2 == ButtonState.Released);
        }

        public bool IsButtonClicked(ButtonState current, ButtonState previous)
        {
            return (current == ButtonState.Pressed
                   && previous == ButtonState.Released);
        }

        #endregion

        #region Released

        public bool IsMiddleButtonReleased()
        {
            return (mouseState.MiddleButton == ButtonState.Released
                   && previousMouseState.MiddleButton == ButtonState.Pressed);
        }

        public bool IsLeftButtonReleased()
        {
            return (mouseState.LeftButton == ButtonState.Released
                   && previousMouseState.LeftButton == ButtonState.Pressed);
        }

        public bool IsRightButtonReleased()
        {
            return (mouseState.RightButton == ButtonState.Released
                   && previousMouseState.RightButton == ButtonState.Pressed);
        }

        public bool IsBackButtonReleased()
        {
            return (mouseState.XButton1 == ButtonState.Released
                   && previousMouseState.XButton1 == ButtonState.Pressed);
        }

        public bool IsForwardButtonReleased()
        {
            return (mouseState.XButton2 == ButtonState.Released
                   && previousMouseState.XButton2 == ButtonState.Pressed);
        }

        public bool IsButtonReleased(ButtonState current, ButtonState previous)
        {
            return (current == ButtonState.Released
                   && previous == ButtonState.Pressed);
        }

        #endregion

        #endregion

        #region Flush

        public void Flush()
        {
            previousMouseState = mouseState;
            previousScrollWheelValue = scrollWheelValue;

            mouseState = Mouse.GetState();
            scrollWheelValue = mouseState.ScrollWheelValue;
        }

        #endregion

    }
}