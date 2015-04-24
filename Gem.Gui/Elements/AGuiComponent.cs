using Gem.Gui.Configuration;
using Gem.Gui.Controls;
using Gem.Gui.Controls.Aggregators;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using Gem.Gui.Transformation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Gem.Gui.Elements
{
    public abstract class AGuiComponent : IGuiComponent
    {

        #region Fields

        private readonly Alignment layoutStyle;
        private readonly RenderTemplate renderTemplate;
        private readonly IControlAggregator aggregator;
        private readonly Control<ElementEventArgs> control;

        private Options options;
        private Region region;
        private IGuiComponent parent;
        private IList<ITransformation> transformations = new List<ITransformation>();
        private GuiSprite currentGuiSprite;


        #endregion

        #region Properties

        public IControl<ElementEventArgs> Events
        {
            get { return control as IControl<ElementEventArgs>; }
        }

        public RenderStyle RenderStyle
        {
            get { return renderTemplate.Style; }
        }

        public GuiSprite Sprite
        {
            get { return currentGuiSprite; }
            protected set { currentGuiSprite = value; }
        }

        public Alignment LayoutStyle
        {
            get { return layoutStyle; }
        }

        public Options Options
        {
            get { return options; }
            set { options = value; }
        }

        public Region Region
        {
            get { return region; }
            set { region = value; }
        }

        public IGuiComponent Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        #endregion

        #region Ctor

        public AGuiComponent(RenderTemplate renderTemplate,
                             Alignment layoutStyle,
                             Region region,
                             ControlTarget target,
                             IGuiComponent parent = null)
        {
            
            this.renderTemplate = renderTemplate;
            this.layoutStyle = layoutStyle;
            this.region = region;
            this.parent = parent;
            this.currentGuiSprite = renderTemplate.Common;

            this.Events.GotFocus += (sender, args) => this.currentGuiSprite = renderTemplate.Focused;
            this.Events.LostFocus += (sender, args) => this.currentGuiSprite = renderTemplate.Common;
            this.Events.Clicked += (sender, args) => this.currentGuiSprite = renderTemplate.Common;

            UseMouse();
        }

        [Conditional("MouseEnabled")]
        public void UseMouse()
        {
            var mouseCaptureTemplate = renderTemplate["MouseCapture"];
            if (mouseCaptureTemplate == null)
            {
                throw new ArgumentNullException("No appropriate sprite for mouse capture was found");
            }

            this.Events.GotMouseCapture += (sender, args) => this.Sprite = mouseCaptureTemplate;
            this.Events.LostMouseCapture += (sender, args) => this.Sprite = renderTemplate.Common;
        }

        #endregion

        #region Public Members

        public void AddTransformation(ITransformation transformation)
        {
            transformations.Add(transformation);
        }


        public virtual void Update(AggregationToken context, double deltaTime)
        {
            aggregator.Aggregate(this, context);

            for (int index = 0; index < transformations.Count(); index++)
            {
                if (transformations[index].Enabled)
                {
                    transformations.RemoveAt(index);
                    continue;
                }
                transformations[index].Transform(this, deltaTime);
            }
        }

        public virtual void Draw(ADrawManager manager)
        {
            manager.Draw(this);
        }

        #endregion



        public Alignment Alignment
        {
            get { throw new NotImplementedException(); }
        }
    }
}
