using Gem.Gui.Styles;
using Gem.Gui.Rendering;
using Gem.Gui.Text;
using Microsoft.Xna.Framework.Graphics;
using System;
using Gem.Gui.Input;
using Microsoft.Xna.Framework;

namespace Gem.Gui.Controls
{
    public class Slider : AControl
    {

        private enum SliderAction
        {
            Add,
            Subtract
        }

        #region Fields

        private readonly Func<bool> Subtract;
        private readonly Func<bool> Add;
        private readonly SliderInfo sliderInfo;
        private readonly SliderDrawable sliderDrawable;

        private SliderAction previousAction;
        private double sliderRepeatTimer;
        private bool scriptHasEvaluated;

        #endregion

        #region Ctor

        public Slider(SliderInfo sliderInfo, SliderDrawable sliderDrawable, Texture2D texture, Region region, ARenderStyle style)
            : base(texture, region, style, null)
        {
            this.sliderInfo = sliderInfo;
            this.sliderDrawable = sliderDrawable;

            Subtract += () => InputManager.GamePad.IsButtonPressed(InputManager.GamePadInputKeys.Left);
            Subtract += () => InputManager.Keyboard.IsKeyPressed(InputManager.KeyboardInputKeys.Left);

            Add += () => InputManager.GamePad.IsButtonPressed(InputManager.GamePadInputKeys.Right);
            Add += () => InputManager.Keyboard.IsKeyPressed(InputManager.KeyboardInputKeys.Right);
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
                        MoveSlider(sliderInfo.Step);
                        sliderDrawable.Move(sliderInfo.PositionPercent);
                    }
                    scriptHasEvaluated = true;
                }
                if (Subtract())
                {
                    if (ShouldHandle(SliderAction.Subtract, deltaTime))
                    {
                        MoveSlider(-sliderInfo.Step);
                        sliderDrawable.MoveByPercentage(sliderInfo.PositionPercent);
                    }
                    scriptHasEvaluated = true;
                }
            }
            scriptHasEvaluated = false;
        }

        public override void Render(SpriteBatch batch)
        {
            base.Render(batch);
            sliderDrawable.Draw(batch, this.RenderParameters);
        }

        #endregion

        #region Private Helpers

        private bool ShouldHandle(SliderAction action, double deltaTime)
        {
            if (this.previousAction != action)
            {
                sliderRepeatTimer = InputManager.KeyRepetition.KeyRepeatStartDuration;
                this.previousAction = action;
                return true;
            }
            if (this.previousAction == action)
            {
                sliderRepeatTimer -= deltaTime;
                if (sliderRepeatTimer <= 0.0f)
                {
                    sliderRepeatTimer += InputManager.KeyRepetition.KeyRepeatDuration;
                    return true;
                }
            }
            return false;
        }

        private void MoveSlider(float step)
        {
            sliderInfo.Move(step);
            OnValueChanged();
        }


        #endregion

    }
}
