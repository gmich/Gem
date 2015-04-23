using Gem.Gui.Configuration;
using Gem.Gui.Controls;
using Gem.Gui.Controls.Aggregators;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using Gem.Gui.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Gui.Elements
{
    public abstract class GuiElement<TEventArgs> : IGuiElement
        where TEventArgs : EventArgs
    {

        #region Fields

        private readonly LayoutStyle layoutStyle;
        private readonly RenderTemplate renderTemplate;

        private Options options;
        private Region region;
        private int order;
        private IGuiElement parent;
        private IList<ITransformation> transformations = new List<ITransformation>();
        private GuiSprite currentGuiSprite;

        protected IControlAggregator aggregator;

        protected Control<TEventArgs> control;

        #endregion

        #region Properties

        public IControl<TEventArgs> Events
        {
            get { return control as IControl<TEventArgs>; }
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
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

        public LayoutStyle LayoutStyle
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

        public IGuiElement Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        #endregion

        #region Ctor

        public GuiElement(RenderTemplate renderTemplate,
                          LayoutStyle layoutStyle,
                          Region region,
                          int order = 0,
                          IGuiElement parent = null)
        {           

            this.renderTemplate = renderTemplate;
            this.layoutStyle = layoutStyle;
            this.region = region;
            this.order = order;
            this.parent = parent;
            this.currentGuiSprite = renderTemplate.Common;

            this.Events.GotFocus += (sender, args) => this.currentGuiSprite = renderTemplate.Focused;
            this.Events.LostFocus += (sender, args) => this.currentGuiSprite = renderTemplate.Common;
            this.Events.Clicked += (sender, args) => this.currentGuiSprite = renderTemplate.Common;
        }

        #endregion

        #region Public Members

        public void AddTransformation(ITransformation transformation)
        {
            transformations.Add(transformation);
        }

        public virtual void Update(double deltaTime)
        {
            aggregator.Aggregate(this);

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

    }
}
