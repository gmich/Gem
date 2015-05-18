using Gem.Gui.Configuration;
using Gem.Gui.Core.Controls;
using Gem.Gui.Styles;
using Gem.Gui.Events;
using Gem.Gui.Rendering;
using Gem.Gui.Text;
using Gem.Gui.Transformations;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Xna.Framework;
using Gem.Gui.Alignment;

namespace Gem.Gui.Controls
{
    /// <summary>
    /// Base class for controls
    /// </summary>
    public abstract class AControl : IAlignable, IScalable, ITransformable
    {

        #region Fields

        private readonly IList<ITransformation> transformations = new List<ITransformation>();

        #endregion

        #region Properties

        public ViewEvents<ControlEventArgs> Events { get; private set; }

        public RenderParameters RenderParameters { get; private set; }

        public Sprite Sprite { get; private set; }

        public ARenderStyle RenderStyle { get; set; }

        public AlignmentContext ScreenAlignment { get; private set; }

        public Options Options { get; private set; }

        public Region Region { get; protected set; }

        public Padding Padding { get; private set; }

        public RenderTemplate RenderTemplate { get; private set; }
        
        private IText text;
        public IText Text
        {
            get { return text; }
            set
            {
                text = value;
                if (text != null)
                {
                    this.Text.OnTextChanged += (sender, args) => this.Align(Settings.ViewRegion);
                    this.Events.SubscribeStyle(this, text.RenderStyle);
                    this.text.Alignment.OnAlignmentChanged += (sender, alignment) => this.Align(Settings.ViewRegion);
                }
            }
        }

        private bool hasFocus;
        public bool HasFocus
        {
            get
            {
                return hasFocus;
            }
            set
            {
                var previousFocus = hasFocus;
                hasFocus = value;

                if (!previousFocus && hasFocus)
                {
                    Events.OnGotFocus();
                }
                else if (previousFocus && !hasFocus)
                {
                    Events.OnLostFocus();
                }
            }
        }

        private bool hasHover;
        public bool HasHover
        {
            get
            {
                return hasHover;
            }
            set
            {
                if (!hasHover && value)
                {
                    Events.OnMouseCapture();
                }
                else if (hasHover && !value)
                {
                    Events.OnLostMouseCapture();
                }
                hasHover = value;
            }
        }

        #endregion

        #region Ctor

        public AControl(Texture2D texture, Region region, ARenderStyle style)
        {
            this.Region = region;
            this.Sprite = new Sprite(texture);
            this.RenderParameters = new RenderParameters();
            this.Options = new Options();
            this.Events = new ViewEvents<ControlEventArgs>(this, () => new ControlEventArgs());
            this.RenderStyle = style;
            this.Events.SubscribeStyle(this, RenderStyle);
            this.Padding = new Padding();
            this.ScreenAlignment = AlignmentContext.Default;
            this.RenderTemplate = RenderTemplate.Default;

            ScreenAlignment.OnAlignmentChanged += (sender, args) => this.Align(Settings.ViewRegion);
            region.onSizeChange += (sender, args) => this.Align(Settings.ViewRegion);
            region.onPositionChange += (sender, args) => this.Align(Settings.ViewRegion);
        }

        #endregion

        #region Virtual Members

        public virtual IEnumerable<AControl> Entries() { yield return this; }

        public virtual void Update(double deltaTime)
        {
            for (int index = 0; index < transformations.Count(); index++)
            {
                if (!transformations[index].Enabled)
                {
                    transformations.RemoveAt(index);
                    continue;
                }
                transformations[index].Transform(this as ITransformable, deltaTime);
            }
            if (Text != null)
            {
                Text.Update(deltaTime);
            }
        }

        public virtual void Render(SpriteBatch batch)
        {
            if (!Options.IsVisible) return;

            RenderTemplate.ControlDrawable.Render(batch, this);
            RenderStyle.Render(batch);

            if (Text != null)
            {
                text.RenderStyle.Render(batch);
                RenderTemplate.TextDrawable.Render(batch, this.Text);
            }
        }

        public virtual void Scale(Vector2 scale)
        {
            Region.Scale(scale);
            RenderParameters.Scale = scale;
            if (Text != null)
            {
                Text.Scale(scale);
            }

            this.Align(Settings.ViewRegion);
        }

        public virtual void Align(Region parent)
        {
            ScreenAlignment.ManageTransformation(this.AddTransformation, parent, this.Region, this.Padding);
            if (Text != null)
            {
                Text.Align(this.Region);
            }
        }

        #endregion
  
        public override string ToString()
        {
            //TODO: implement
            return base.ToString();
        }

        public System.IDisposable AddTransformation(ITransformation transformation)
        {
            if (!Options.AllowTransformations)
            {
                return Gem.Infrastructure.Disposable.CreateDummy();
            }
            transformations.Add(transformation);

            return Gem.Infrastructure.Disposable.Create(transformations, transformation);
        }
    }
}
