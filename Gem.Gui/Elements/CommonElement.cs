using Gem.Gui.Configuration;
using Gem.Gui.Controls;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using Gem.Gui.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Elements
{
    public abstract class CommonElement<TControl, TEventArgs> : IGuiElement
        where TControl : IControl<TEventArgs>
        where TEventArgs : EventArgs
    {

        #region Fields

        private readonly LayoutStyle layoutStyle;

        private RenderTemplate renderTemplate;
        private Options options;
        private Region region;
        private int order;
        private IGuiElement parent;
        private IList<ITransformation> transformations = new List<ITransformation>();
        private GuiSprite currentGuiSprite;

        #endregion

        #region Properties

        public TControl Events
        {
            get;
            set;
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public RenderTemplate RenderTemplate
        {
            get { return renderTemplate; }
            set { renderTemplate = value; }
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

        public CommonElement(RenderTemplate renderTemplate,
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

        public void Update(double deltaTime)
        {
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

        public void Draw(IDrawManager manager)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
