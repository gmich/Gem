using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Gem.Gui.Styles;
using Gem.Gui.Rendering;

namespace Gem.Gui.Text
{
    using Alignment;

    public class StandardText : IText
    {

        #region Fields

        private readonly SpriteFont font;
        public event EventHandler<TextEventArgs> OnTextChanged;

        #endregion

        #region Ctor

        public StandardText(SpriteFont font, Vector2 position, string value)
            : this(font, position, value, AlignmentContext.Default)
        { }

        public StandardText(SpriteFont font, Vector2 position, string value, AlignmentContext alignment = null)
        {
            this.font = font;
            this.Value = value;
            this.Alignment = alignment;
            this.RenderParameters = new RenderParameters();
            this.Alignment = alignment ?? AlignmentContext.Default;
            this.Region = new Region(position, Font.MeasureString(value));
            this.RenderStyle = new NoStyle();
        }

        #endregion

        #region Properties

        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                OnTextChangedAggregation(new TextEventArgs
                {
                    PreviousText = _value,
                    NewText = value
                });
                _value = value;
            }
        }

        public SpriteFont Font
        {
            get { return font; }
        }

        public Region Region
        {
            get;
            set;
        }

        public Padding Padding
        {
            get;
            set;
        }

        public RenderParameters RenderParameters
        {
            get;
            set;
        }

        public IRenderStyle RenderStyle
        {
            get;
            set;
        }

        public AlignmentContext Alignment
        {
            get;
            set;
        }

        #endregion

        private void OnTextChangedAggregation(TextEventArgs eventArgs)
        {
            var handler = OnTextChanged;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

    }
}
