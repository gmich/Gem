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

namespace Gem.Gui.Controls
{
    /// <summary>
    /// Base class for controls
    /// </summary>
    public abstract class AControl
    {

        #region Fields

        private readonly IList<ITransformation> transformations = new List<ITransformation>();

        #endregion

        #region Properties

        public ViewEvents<ControlEventArgs> Events { get; private set; }

        public RenderParameters RenderParameters { get; private set; }

        public Sprite Sprite { get; private set; }

        public IRenderStyle RenderStyle { get; set; }

        private IText text;
        public IText Text
        {
            get { return text; }
            set
            {
                text = value;
                if (text != null)
                {
                    this.Events.SubscribeStyle(this, text.RenderStyle);
                    this.text.Alignment.OnAlignmentChanged += (sender, alignment) => this.Align(Settings.ViewRegion);
                }
            }
        }

        public Options Options { get; private set; }

        public Region Region { get; private set; }

        public Padding Padding { get; private set; }

        #endregion

        #region Ctor

        public AControl(Texture2D texture, Region region, IRenderStyle style)
        {
            this.Region = region;
            this.Sprite = new Sprite(texture);
            this.RenderParameters = new RenderParameters();
            this.Options = new Options();
            this.Events = new ViewEvents<ControlEventArgs>(this, () => new ControlEventArgs());
            this.RenderStyle = style;
            this.Events.SubscribeStyle(this, RenderStyle);

            region.onSizeChange += (sender, args) => this.Align(Settings.ViewRegion);
        }

        #endregion

        #region Public Members

        public virtual void Align(Region viewPort)
        {
            if (Text != null)
            {
                var transformation = Text.Alignment.GetAlignementTransformation(this.Region, Text.Region, Text.Padding);
                Text.Alignment.ActiveTransformations(this.AddTransformation(transformation));
            }
        }

        public System.IDisposable AddTransformation(ITransformation transformation)
        {
            transformations.Add(transformation);
            return Gem.Infrastructure.Disposable.Create(transformations, transformation);
        }

        public virtual void Update(double deltaTime)
        {
            for (int index = 0; index < transformations.Count(); index++)
            {
                if (!transformations[index].Enabled)
                {
                    transformations.RemoveAt(index);
                    continue;
                }
                transformations[index].Transform(this, deltaTime);
            }
        }

        public virtual void Render(SpriteBatch batch, RenderTemplate template)
        {
            if (!Options.IsVisible) return;

            template.ControlDrawable.Render(batch, this);
            RenderStyle.Render(batch);

            if (Text != null)
            {
                text.RenderStyle.Render(batch);
                template.TextDrawable.Render(batch, this.Text);
            }
        }

        public override string ToString()
        {
            //TODO: implement
            return base.ToString();
        }

        #endregion


    }
}
