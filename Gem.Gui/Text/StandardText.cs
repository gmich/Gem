using System;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Text
{
    using Alignment;

    public class StandardText : IText
    {
        private readonly SpriteFont font;

        public event EventHandler<TextEventArgs> OnTextChanged;

        public StandardText(SpriteFont font, string value)
            : this(font, value, AlignmentContext.Default)
        { }

        public StandardText(SpriteFont font, string value, AlignmentContext alignment)
        {
            this.font = font;
            this.Value = value;
            this.Alignment = alignment;
        }

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

        private AlignmentContext _alignment;
        public AlignmentContext Alignment
        {
            get { return _alignment; }
            set { _alignment = value; }
        }

        internal void OnTextChangedAggregation(TextEventArgs eventArgs)
        {
            var handler = OnTextChanged;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }
    }
}
