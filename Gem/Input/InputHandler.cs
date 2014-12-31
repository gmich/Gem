using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Gem.Input
{

    /// <summary>
    /// A static instance to handle keyboard and mouse inputs
    /// </summary>
    static public class InputHandler
    {
        #region Constants and Fields

        /// <summary>
        /// The keyboard state.
        /// </summary>
        static private KeyboardState keyboardState;

        /// <summary>
        /// The last keyboard state.
        /// </summary>
        static private KeyboardState lastKeyboardState;

        /// <summary>
        /// The last mouse state.
        /// </summary>
        static private MouseState lastMouseState;

        /// <summary>
        /// The mouse state.
        /// </summary>
        static private MouseState mouseState;


        static private float prevWheelValue;
        static private float currWheelValue;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManager"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        /// <param name="resolutionManager">
        /// The resolution manager.
        /// </param>
        static public void Initialize()
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            currWheelValue = mouseState.ScrollWheelValue;
            prevWheelValue = currWheelValue;
        }

        #endregion

        #region Mouse Properties

        /// <summary>
        /// Gets MousePosition.
        /// </summary>
        static public Vector2 MousePosition
        {
            get
            {
                return new Vector2(mouseState.X, mouseState.Y);
            }
        }

        static public Rectangle MouseRectangle
        {
            get
            {
                return new Rectangle((int)MousePosition.X,(int)MousePosition.Y,1,1);
            }
        }


        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The flush.
        /// </summary>
        static public void Flush()
        {
            lastKeyboardState = keyboardState;
            lastMouseState = mouseState;
            prevWheelValue = currWheelValue;
        }

        #region Mouse State

        static public bool IsWheelMovingUp()
        {
            return ((currWheelValue / 120 - prevWheelValue / 120) > 0) ;           
        }

        static public bool IsWheelMovingDown()
        {
            return ((currWheelValue / 120 - prevWheelValue / 120) < 0);
        }


        static public bool MouseMiddleButtonClicked()
        {
            return (mouseState.MiddleButton == ButtonState.Pressed);
        }

        /// <summary>
        /// The left button is clicked.
        /// </summary>
        /// <returns>
        /// The left button is clicked.
        /// </returns>
        static public bool LeftButtonIsReleased()
        {
            return mouseState.LeftButton == ButtonState.Released
                   && lastMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// The right button is clicked.
        /// </summary>
        /// <returns>
        /// The right button is clicked.
        /// </returns>
        static public bool RightButtonIsClicked()
        {
            return mouseState.RightButton == ButtonState.Pressed;
        }

        static public bool LeftButtonIsClicked()
        {
            return mouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// The right button is clicked.
        /// </summary>
        /// <returns>
        /// The right button is clicked.
        /// </returns>
        static public bool RightButtonIsReleased()
        {
            return mouseState.RightButton == ButtonState.Released
                   && lastMouseState.RightButton == ButtonState.Pressed;
        }

        static public bool BackButtonIsPressed()
        {
            return mouseState.XButton1 == ButtonState.Pressed
                    && lastMouseState.XButton1 == ButtonState.Released;
        }

        static public bool ForwardButtonIsPressed()
        {
            return mouseState.XButton2 == ButtonState.Pressed
                    && lastMouseState.XButton2 == ButtonState.Released;
        }
        #endregion

        /// <summary>
        /// The is key down.
        /// </summary>
        /// <param name="keyToTest">
        /// The key to test.
        /// </param>
        /// <returns>
        /// The is key down.
        /// </returns>
        /// 

        #region Keyboard State

        static public bool IsKeyDown(Keys keyToTest)
        {
            return keyboardState.IsKeyDown(keyToTest);
        }

        /// <summary>
        /// The is key pressed.
        /// </summary>
        /// <param name="keyToTest">
        /// The key to test.
        /// </param>
        /// <returns>
        /// The is key pressed.
        /// </returns>
        static public bool IsKeyPressed(Keys keyToTest)
        {
            return keyboardState.IsKeyUp(keyToTest) && lastKeyboardState.IsKeyDown(keyToTest);
        }

        /// <summary>
        /// The is key released.
        /// </summary>
        /// <param name="keyToTest">
        /// The key to test.
        /// </param>
        /// <returns>
        /// The is key released.
        /// </returns>
        static public bool IsKeyReleased(Keys keyToTest)
        {
            return keyboardState.IsKeyDown(keyToTest) && lastKeyboardState.IsKeyUp(keyToTest);
        }

        #endregion
        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        static public void Update(GameTime gameTime)
        {
            Flush();

            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            currWheelValue = mouseState.ScrollWheelValue;
        }

        #endregion
    }
}