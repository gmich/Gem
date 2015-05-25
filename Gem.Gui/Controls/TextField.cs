using Gem.Gui.Alignment;
using Gem.Gui.Events;
using Gem.Gui.Input;
using Gem.Gui.Rendering;
using Gem.Gui.Styles;
using Gem.Gui.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Gem.Gui.Controls
{

    public class TextField : AControl
    {
        #region Fields

        private readonly SpriteFont font;
        private readonly TextAppenderHelper appender;
        private readonly Timer timer;
        private int cursorIndex;
        private bool showCursor;

        private IText cursor;
        private IText hint;
        private Keys pressedKey;

        #endregion

        #region Events

        public event EventHandler<TextFieldEventArgs> OnTextChanged;
        public event EventHandler<string> OnTextEntered;

        #endregion

        #region Ctor

        internal TextField(TextAppenderHelper appender,
                         SpriteFont font,
                         Texture2D texture,
                         Region region,
                         Color textcolor,
                         ARenderStyle style,
                         string hint,
                         AlignmentContext alignmentContext)
            : base(texture, region, style)
        {
            int padding = 5;
            this.font = font;
            this.appender = appender;
            Events.LostFocus += (sender, args) => ShouldProcessInput = false;
            Events.GotFocus += (sender, args) => ShouldProcessInput = true;

            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler((sender, args) => showCursor = !showCursor);
            timer.Interval = appender.CursorFlickInterval;
            timer.Enabled = true;

            this.hint = new StandardText(font, Vector2.Zero, hint, alignmentContext);
            this.hint.RenderParameters.Color = new Color(textcolor.R, textcolor.G, textcolor.B, 0.4f);
            Text = new StandardText(font, Vector2.Zero, null, alignmentContext);
            Text.RenderParameters.Color = textcolor;
            Text.Padding.Left = padding;
            Text.Padding.Right = padding;
            this.hint.Padding.Left = padding;
            SetupCursor();

            //check if the buffer is not full
            appender.ShouldHandleKey += (key, keyToChar) =>
                ((Text.Region.Frame.Right + Text.Padding.Left + Text.Padding.Right 
                + (font.MeasureString(keyToChar.ToString()).X) * Configuration.Settings.Scale.X)
                < (this.Region.Frame.Right));
        }

        #endregion

        #region Event Aggregation

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

        #endregion

        #region Properties

        private bool _shouldProcessInput;
        private bool ShouldProcessInput
        {
            get { return _shouldProcessInput; }
            set
            {
                if (!value & _shouldProcessInput)
                {
                    OnTextEnteredAggregation(this.Text.Value);
                }
                _shouldProcessInput = value;
            }
        }

        #endregion

        #region Public Methods

        public void InsertText(string text)
        {
            foreach (var character in text)
            {
                InsertChar(character);
            }
        }
        public void RemoveCharacters(int count)
        {
            for (int i = 0; i < count; i++)
            {
                RemoveChar();
            }
        }

        private string GetStringAtCursor()
        {
            if (Text.Value != string.Empty)
            {
                return (cursorIndex - 1 > 0)
                    ? Text.Value[cursorIndex - 1].ToString()
                    : Text.Value[0].ToString();

            }
            return "a";
        }

        #endregion

        #region Private Helper Methods

        private void AlignCursor()
        {
            cursor.Alignment.ManageTransformation(this.AddTransformation, this.Region, cursor.Region, cursor.Padding);
        }

        private void SetupCursor()
        {
            Text.OnTextChanged += (sender, args) => AlignCursor();

            cursor = new StandardText(this.font,
                                      Region.Position,
                                      appender.Cursor.ToString(),
                                      new AlignmentContext(HorizontalAlignment.RelativeTo(() =>
                                          {
                                              string currentCursor = GetStringAtCursor();

                                              return Text.Region.Position.X
                                                  + (font.MeasureString(Text.Value.Substring(0, cursorIndex)).X
                                                  - font.MeasureString(currentCursor).X / 2)
                                                  * Configuration.Settings.Scale.X;
                                          }), Text.Alignment.VerticalAlignment,
                                              AlignmentTransition.Instant));
            cursor.RenderParameters.Color = Text.RenderParameters.Color;
        }

        private void InsertChar(char charToInsert)
        {
            cursorIndex++;
            Text.Value = Text.Value.Insert((cursorIndex - 1), new string(charToInsert, 1));
            OnTextChangedAggregation(new TextFieldEventArgs(this.Text.Value, charToInsert));
        }

        private void RemoveChar()
        {
            if (cursorIndex > 0)
            {
                char charToRemove = Text.Value[--cursorIndex];
                Text.Value = Text.Value.Remove(cursorIndex, 1);
                OnTextChangedAggregation(new TextFieldEventArgs(this.Text.Value, charToRemove));
            }
        }

        private void ProcessKeyInputs(double deltaTime)
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
                    if (!appender.ShouldHandleKey(key, convertedChar))
                    {
                        continue;
                    }
                    InsertChar(convertedChar);
                }
                else
                {
                    switch (key)
                    {
                        case Keys.Back:
                            RemoveChar();
                            break;
                        case Keys.Delete:
                            if (cursorIndex < Text.Value.Length)
                            {
                                char charToRemove = Text.Value[cursorIndex];
                                Text.Value = Text.Value.Remove(cursorIndex, 1);
                                OnTextChangedAggregation(new TextFieldEventArgs(this.Text.Value, charToRemove));
                            }
                            break;
                        case Keys.Left:
                            if (cursorIndex > 0)
                            {
                                cursorIndex--;
                                AlignCursor();
                            }
                            break;
                        case Keys.Right:
                            if (cursorIndex < Text.Value.Length)
                            {
                                cursorIndex++;
                                AlignCursor();
                            }
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

        #endregion

        #region AControl Members

        public override void Scale(Vector2 scale)
        {
            base.Scale(scale);
            cursor.Scale(scale);
            hint.Scale(scale);
        }

        public override void Align(Region viewPort)
        {
            base.Align(viewPort);

            AlignContent();
        }

        private void AlignContent()
        {
            Text.Region.Position = Text.Alignment.GetTargetRegion(this.Region, Text.Region, Text.Padding).Position;
            hint.Region.Position = hint.Alignment.GetTargetRegion(this.Region, hint.Region, hint.Padding).Position;
            cursor.Region.Position = cursor.Alignment.GetTargetRegion(this.Region, cursor.Region, cursor.Padding).Position;
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            if (!HasFocus) return;

            if (appender.Input.IsKeyClicked(InputManager.KeyboardInputKeys.Trigger))
            {
                ShouldProcessInput = !ShouldProcessInput;
            }

            if (ShouldProcessInput)
            {
                ProcessKeyInputs(deltaTime);
            }
        }

        public override void Render(SpriteBatch batch)
        {
            base.Render(batch);

            if (showCursor && ShouldProcessInput)
            {
                cursor.RenderStyle.Render(this,batch);
                RenderTemplate.TextDrawable.Render(batch, this.cursor);
            }
            if (Text.Value == string.Empty)
            {
                hint.RenderStyle.Render(this,batch);
                RenderTemplate.TextDrawable.Render(batch, this.hint);
            }
        }

        #endregion

    }
}
