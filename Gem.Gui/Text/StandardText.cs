using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Gem.Gui.Styles;
using Gem.Gui.Rendering;
using Gem.Gui.Alignment;
using Gem.Gui.Transformations;
using System.Collections.Generic;

namespace Gem.Gui.Text
{

    public class StandardText : IText
    {

        #region Fields

        private readonly SpriteFont font;
        private readonly IList<ITransformation> transformations = new List<ITransformation>();
        private readonly string defaultSize = "A";

        #endregion

        #region Ctor

        public StandardText(SpriteFont font, Vector2 position, string value)
            : this(font, position, value, AlignmentContext.Default)
        { }

        public StandardText(SpriteFont font, Vector2 position, string value, AlignmentContext alignment = null)
        {
            this.font = font;
            this.RenderParameters = new RenderParameters();
            this.Region = new Region(position, Font.MeasureString(value ?? defaultSize) * RenderParameters.Scale);
            this.Value = value ?? string.Empty;
            this.Alignment = alignment ?? AlignmentContext.Default;
            this.RenderStyle = new NoStyle();
            this.Padding = Padding.Zero;
            this.OnTextChanged += (sender, args) =>
            {
                string textToMeasure = (String.IsNullOrEmpty(args.NewText)) ? defaultSize : args.NewText;
                this.Region = new Region(Region.Position, Font.MeasureString(textToMeasure) * Configuration.Settings.Scale);
            };
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
            private set;
        }

        public RenderParameters RenderParameters
        {
            get;
            private set;
        }

        public ARenderStyle RenderStyle
        {
            get;
            private set;
        }

        public AlignmentContext Alignment
        {
            get;
            private set;
        }

        #endregion

        #region Events

        public event EventHandler<TextEventArgs> OnTextChanged;

        private void OnTextChangedAggregation(TextEventArgs eventArgs)
        {
            var handler = OnTextChanged;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

        #endregion

        #region IText Members

        public void Align(Region parent)
        {
            this.Alignment.ManageTransformation(this.AddTransformation, parent, this.Region, this.Padding);
        }

        public void Scale(Vector2 scale)
        {
            RenderParameters.Scale = scale;
            Region.Scale(scale);

            string textToMeasure = (Value == string.Empty) ? defaultSize : Value;
            Region.Size = Font.MeasureString(textToMeasure) * scale;
        }

        public IDisposable AddTransformation(ITransformation transformation)
        {
            transformations.Add(transformation);

            return Gem.Infrastructure.Disposable.Create(transformations, transformation);
        }

        public void Update(double deltaTime)
        {
            for (int index = 0; index < transformations.Count; index++)
            {
                if (!transformations[index].Enabled)
                {
                    transformations.RemoveAt(index);
                    continue;
                }
                transformations[index].Transform(this, deltaTime);
            }
        }

        #endregion
    }
}
