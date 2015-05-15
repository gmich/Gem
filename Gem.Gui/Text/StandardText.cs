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
           

            this.Region = new Region(position, Font.MeasureString(value) * RenderParameters.Scale);
            this.RenderStyle = new NoStyle();
            Padding = Padding.Zero;
            
            //TODO: refactor scaling
            //this.RenderParameters.OnScaleChange += (sender, args) => this.Region = new Region(position, Font.MeasureString(value) * RenderParameters.Scale);
            this.OnTextChanged+=(sender, args) => this.Region = new Region(Region.Position, Font.MeasureString(args.NewText));
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
                _value = value;
                OnTextChangedAggregation(new TextEventArgs
                {
                    PreviousText = _value,
                    NewText = value
                });
            }
        }

        public SpriteFont Font
        {
            get { return font; }
        }

        public Region Region
        {
            get;
            private set;
        }

        public Padding Padding
        {
            get;
            set;
        }

        public RenderParameters RenderParameters
        {
            get;
            private set;
        }

        public ARenderStyle RenderStyle
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
