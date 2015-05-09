using System;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Text
{
    using Alignment;
    using Microsoft.Xna.Framework;
    using Gem.Gui.Core.Styles;
    using Gem.Gui.Rendering;

    public class StandardText : IText
    {

        #region Fields

        private readonly SpriteFont font;
        public event EventHandler<TextEventArgs> OnTextChanged;

        #endregion

        #region Ctor

        public StandardText(SpriteFont font, string value)
            : this(font, value, AlignmentContext.Default)
        { }

        public StandardText(SpriteFont font, string value, AlignmentContext alignment)
        {
            this.font = font;
            this.Value = value;
            this.Alignment = alignment;
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

        private Region _region;
        public Region Region
        {
            get { return _region; }
            set { _region = value; }
        }
        private RenderParameters _renderParameters;
        public RenderParameters RenderParameters 
        {
            get { return _renderParameters; }
            set { _renderParameters = value; }
        }

        private IRenderStyle _renderStyle;
        public IRenderStyle RenderStyle
        {
            get { return _renderStyle; }
            set { _renderStyle = value; }
        }

        private AlignmentContext _alignment;
        public AlignmentContext Alignment
        {
            get { return _alignment; }
            set { _alignment = value; }
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
