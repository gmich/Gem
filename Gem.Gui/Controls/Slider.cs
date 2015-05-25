using Gem.Gui.Styles;
using Gem.Gui.Rendering;
using Gem.Gui.Text;
using Microsoft.Xna.Framework.Graphics;
using System;
using Gem.Gui.Input;
using Gem.Gui.Utilities;
using Microsoft.Xna.Framework;

namespace Gem.Gui.Controls
{
    public class Slider : AControl
    {

        private enum SliderAction
        {
            None,
            Add,
            Subtract
        }

        #region Fields

        private readonly Func<bool> Subtract;
        private readonly Func<bool> Add;
        private readonly SliderInfo sliderInfo;
        private readonly SliderDrawable sliderDrawable;
        private readonly KeyRepetition keyRepetition;

        private SliderAction sliderAction;
        private double sliderRepeatTimer;
        private bool scriptHasEvaluated;
        private bool sliderIsLocked;
        private float sliderOffsetX;

        #endregion

        #region Ctor

        public Slider(SliderInfo sliderInfo, SliderDrawable sliderDrawable, Texture2D texture, Region region, ARenderStyle style)
            : base(texture, region, style, null)
        {
            this.sliderInfo = sliderInfo;
            this.sliderInfo.OnPositionChanged += (sender, args) => OnValueChanged();

            this.sliderDrawable = sliderDrawable;
            this.keyRepetition = new KeyRepetition { KeyRepeatDuration = 0.01d, KeyRepeatStartDuration = 0.3d };

            Subtract += () => InputManager.Mouse.IsWheelMovingUp() ||
                              InputManager.GamePad.IsButtonPressed(InputManager.GamePadInputKeys.Left) ||
                              InputManager.Keyboard.IsKeyPressed(InputManager.KeyboardInputKeys.Left);

            Add += () => InputManager.Mouse.IsWheelMovingDown() ||
                         InputManager.GamePad.IsButtonPressed(InputManager.GamePadInputKeys.Right) ||
                         InputManager.Keyboard.IsKeyPressed(InputManager.KeyboardInputKeys.Right);
        }

        #endregion

        #region Events

        public event EventHandler<float> OnValueChange;

        private void OnValueChanged()
        {
            var handler = OnValueChange;
            if (handler != null)
            {
                handler(this, sliderInfo.Position);
            }
        }

        #endregion

        #region Public Methods

        public Region SliderRegion
        {
            get { return sliderDrawable.SliderRegion; }
        }

        public float SliderValue
        {
            get { return sliderInfo.Position; }
        }

        #endregion

        #region AControl Members

        public override void Align(Region parent)
        {
            base.Align(parent);
            sliderDrawable.Align(this.Region);
        }

        public override void Scale(Vector2 scale)
        {
            base.Scale(scale);
            sliderDrawable.Scale(scale);
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            sliderDrawable.Update(deltaTime);

            if (this.HasFocus)
            {
                if (Add())
                {
                    if (ShouldHandle(SliderAction.Add, deltaTime))
                    {
                        sliderInfo.Move(sliderInfo.Step);
                        sliderDrawable.MoveByPercentage(sliderInfo.PositionPercent);
                    }
                    scriptHasEvaluated = true;
                }
                if (Subtract())
                {
                    if (ShouldHandle(SliderAction.Subtract, deltaTime))
                    {
                        sliderInfo.Move(-sliderInfo.Step);
                        sliderDrawable.MoveByPercentage(sliderInfo.PositionPercent);
                    }
                    scriptHasEvaluated = true;
                }
            }
            sliderInfo.SetPositionByPercentage(sliderDrawable.Percentage);
            CheckMouseIntegration();

            sliderAction = scriptHasEvaluated ? sliderAction : SliderAction.None;

            scriptHasEvaluated = false;
        }

        public override void Render(SpriteBatch batch)
        {
            base.Render(batch);
            sliderDrawable.Draw(batch, this.RenderParameters);
        }

        #endregion

        #region Private Helpers
        
        private void CheckMouseIntegration()
        {
            if (InputManager.Mouse.IsLeftButtonPressed())
            {
                if (sliderIsLocked)
                {
                    sliderDrawable.MoveToLocationSmoothly(Input.InputManager.Mouse.MousePosition.X - sliderOffsetX);
                }
            }
            if (InputManager.Mouse.IsLeftButtonClicked())
            {
                Point mousePosition = Input.InputManager.Mouse.MousePosition;
                if (sliderDrawable.SliderRegion.Frame.Intersects(mousePosition))
                {
                    sliderOffsetX = sliderDrawable.SliderRegion.Frame.IntersectionDepth(mousePosition).X;
                    sliderIsLocked = true;
                }
                if (this.Region.Frame.Intersects(mousePosition))
                {
                    sliderDrawable.MoveToLocationSmoothly(mousePosition.X - sliderDrawable.SliderRegion.Frame.Width / 2);
                }
            }
            if (InputManager.Mouse.IsLeftButtonReleased())
            {
                sliderIsLocked = false;
            }
        }

        private bool ShouldHandle(SliderAction action, double deltaTime)
        {
            if (this.sliderAction != action)
            {
                sliderRepeatTimer = keyRepetition.KeyRepeatStartDuration;
                this.sliderAction = action;
                return true;
            }
            if (this.sliderAction == action)
            {
                sliderRepeatTimer -= deltaTime;
                if (sliderRepeatTimer <= 0.0f)
                {
                    sliderRepeatTimer += keyRepetition.KeyRepeatDuration;
                    return true;
                }
            }
            return false;
        }

        #endregion

    }
}
