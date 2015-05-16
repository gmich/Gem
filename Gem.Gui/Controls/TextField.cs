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

        private int cursorIndex;
        private bool shouldProcessInput;
        private bool showCursor;

        private Keys pressedKey;
        private IText line;
        private IText cursor;
        private Timer timer;

        #endregion

        #region Events

        public event EventHandler<TextFieldEventArgs> OnTextChanged;
        public event EventHandler<string> OnTextEntered;

        #endregion

        #region Ctor

        public TextField(TextAppenderHelper appender,
                         SpriteFont font,
                         Texture2D texture,
                         Region region,
                         Color textcolor,
                         ARenderStyle style,
                         AlignmentContext alignmentContext)
            : base(texture, region, style)
        {
            this.font = font;
            this.appender = appender;
            this.Events.LostFocus += (sender, args) =>
            {
                shouldProcessInput = false;
                OnTextEnteredAggregation(this.line.Value);
            };

            this.Events.Clicked += (sender, args) => shouldProcessInput = !shouldProcessInput;

            this.timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler((sender,args) => showCursor = !showCursor);
            timer.Interval = appender.CursorFlickInterval;
            timer.Enabled = true;

            line = new StandardText(font, region.Position, string.Empty, alignmentContext);
            line.RenderParameters.Color = textcolor;
            SetupCursor();

            //check if the buffer is not full
            appender.ShouldHandleKey += (key, keyToChar) =>
                (line.Region.Frame.Right + cursor.Region.Size.X + font.MeasureString(keyToChar.ToString()).X)
                < this.Region.Frame.Right;
        }

        #endregion

        #region EventAggregation

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

        #region Private Helpers

        private void AlignCursor()
        {
            cursor.Alignment.ManageTransformation(this.AddTransformation, this.Region, cursor.Region, cursor.Padding);
        }

        private void SetupCursor()
        {
            line.OnTextChanged += (sender, args) => AlignCursor();

            cursor = new StandardText(this.font,
                                      Region.Position,
                                      appender.Cursor.ToString(),
                                      new AlignmentContext(HorizontalAlignment.RelativeTo(() => line.Region.Position.X + font.MeasureString(line.Value.Substring(0, cursorIndex)).X),
                                                           VerticalAlignment.RelativeTo(() => line.Region.Frame.Top),
                                                           AlignmentTransition.Fixed));
            cursor.RenderParameters.Color = line.RenderParameters.Color;
        }

        private void InsertChar(char charToInsert)
        {
            cursorIndex++;
            line.Value = line.Value.Insert((cursorIndex-1), new string(charToInsert, 1));
            OnTextChangedAggregation(new TextFieldEventArgs(this.line.Value, charToInsert));
        }

        private void RemoveChar()
        {
            if (cursorIndex > 0)
            {
                char charToRemove = line.Value[--cursorIndex];
                line.Value = line.Value.Remove(cursorIndex, 1);
                OnTextChangedAggregation(new TextFieldEventArgs(this.line.Value, charToRemove));
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
                            if (cursorIndex < line.Value.Length)
                            {
                                char charToRemove = line.Value[cursorIndex];
                                line.Value = line.Value.Remove(cursorIndex, 1);
                                OnTextChangedAggregation(new TextFieldEventArgs(this.line.Value, charToRemove));
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
                            if (cursorIndex < line.Value.Length)
                            { 
                                cursorIndex++;
                                AlignCursor();
                            }
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

        #endregion

        #region AControl Members

        public override void Align(Region viewPort)
        {
            line.Region.Position = line.Alignment.GetTargetRegion(this.Region, line.Region, line.Padding).Position;
            cursor.Region.Position = cursor.Alignment.GetTargetRegion(this.Region, cursor.Region, cursor.Padding).Position;

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

            if (showCursor && shouldProcessInput)
            {
                cursor.RenderStyle.Render(batch);
                template.TextDrawable.Render(batch, this.cursor);
            }
        }

        #endregion

    }
}
