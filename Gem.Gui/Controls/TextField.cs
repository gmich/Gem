using Gem.Gui.Events;
using Gem.Gui.Input;
using Gem.Gui.Rendering;
using Gem.Gui.Styles;
using Gem.Gui.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Gem.Gui.Controls
{

    public class TextField : AControl
    {
        private readonly SpriteFont font;
        private readonly TextAppenderHelper appender;
        private Keys pressedKey;
        private int cursorIndex;
        private bool shouldProcessInput;

        public event EventHandler<TextFieldEventArgs> OnTextChanged;
        public event EventHandler<string> OnTextEntered;
        private IText line;

        public TextField(TextAppenderHelper appender,
                         SpriteFont font,
                         Texture2D texture,
                         Region region,
                         Color textcolor,
                         ARenderStyle style,
                         Alignment.AlignmentContext alignmentContext)
            : base(texture, region, style)
        {
            this.font = font;
            this.appender = appender;
            this.Events.LostFocus += (sender, args) =>
            {
                shouldProcessInput = false;
                OnTextEnteredAggregation(this.line.Value);
            };

            this.Events.GotFocus += (sender, args) => shouldProcessInput = true;
            line = new StandardText(font, Vector2.Zero, string.Empty);
            line.Alignment = alignmentContext;
            line.RenderParameters.Color = textcolor;
            //check if the buffer is not full
            appender.ShouldHandleKey += key => (line.Region.Frame.Right) < this.Region.Frame.Right;
        }

        private void OnTextChangedAggregation(TextFieldEventArgs args)
        {
            var handler = OnTextChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnTextEnteredAggregation(string newText)
        {
            var handler = OnTextEntered;
            if (handler != null)
            {
                handler(this, newText);
            }
        }

        public void ProcessKeyInputs(double deltaTime)
        {
            var pressedKeys = appender.Input.GetPressedKeys();

            bool isShiftPressed = appender.Input.IsKeyPressed(Keys.LeftShift) ||
                                  appender.Input.IsKeyPressed(Keys.RightShift);

            foreach (Keys key in pressedKeys)
            {
                if (!ShouldHandleKey(key, deltaTime)) continue;

                char convertedChar;
                if (KeyboardUtils.KeyToString(key, isShiftPressed, out convertedChar))
                {
                    if (!appender.ShouldHandleKey(key)) continue;
                    line.Value = line.Value.Insert(cursorIndex, new string(convertedChar, 1));
                    OnTextChangedAggregation(new TextFieldEventArgs(this.line.Value, convertedChar));
                    cursorIndex++;
                }
                else
                {
                    switch (key)
                    {
                        case Keys.Back:
                            if (cursorIndex > 0)
                                line.Value = line.Value.Remove(--cursorIndex, 1);
                            break;
                        case Keys.Delete:
                            if (cursorIndex < line.Value.Length)
                            {
                                char charToRemove = line.Value[cursorIndex];
                                line.Value = line.Value.Remove(cursorIndex, 1);
                                OnTextChangedAggregation(new TextFieldEventArgs(this.line.Value, charToRemove));
                            }
                            break;
                        case Keys.Left:
                            if (cursorIndex > 0)
                                cursorIndex--;
                            break;
                        case Keys.Right:
                            if (cursorIndex < line.Value.Length)
                                cursorIndex++;
                            break;
                        case Keys.Enter:
                            this.HasFocus = false;
                            break;
                    }
                }
            }
        }

        private bool ShouldHandleKey(Keys key, double timeDelta)
        {
            if (appender.Input.IsKeyClicked(key))
            {
                appender.KeyRepeatTimer = appender.KeyRepeatStartDuration;
                pressedKey = key;
                return true;
            }

            if (key == pressedKey)
            {
                appender.KeyRepeatTimer -= timeDelta;
                if (appender.KeyRepeatTimer <= 0.0f)
                {
                    appender.KeyRepeatTimer += appender.KeyRepeatDuration;
                    return true;
                }
            }
            return false;
        }

        public override void Align(Region viewPort)
        {
            line.Alignment.ManageTransformation(this.AddTransformation, this.Region, line.Region, line.Padding);

            base.Align(viewPort);
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            if (shouldProcessInput)
            {
                ProcessKeyInputs(deltaTime);
            }
        }

        public override void Render(SpriteBatch batch, RenderTemplate template)
        {
            base.Render(batch, template);

            line.RenderStyle.Render(batch);
            template.TextDrawable.Render(batch, this.line);

        }
    }
}
