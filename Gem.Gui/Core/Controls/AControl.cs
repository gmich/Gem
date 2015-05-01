using Gem.Gui.Alignment;
using Gem.Gui.Configuration;
using Gem.Gui.Controls;
using Gem.Gui.Core.Controls;
using Gem.Gui.Events;
using Gem.Gui.Rendering;
using Gem.Gui.Transformation;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Gui.Controls
{
    /// <summary>
    /// Base class for controls
    /// </summary>
    public abstract class AControl
    {

        #region Fields

        private readonly RenderTemplate renderTemplate;
        private readonly ViewEvents<ControlEventArgs> control;
        private IList<ITransformation> transformations = new List<ITransformation>();
        
        #endregion

        #region Properties

        public ViewEvents<ControlEventArgs> Events
        {
            get { return control; }
        }

        public bool HasHover { get; private set; }
        public bool HasFocus { get; set; }

        public RenderParameters RenderStyle
        {
            get { return renderTemplate.Style; }
        }


        public Options Options
        {
            get;
            set;
        }

        public Region Region
        {
            get;
            set;
        }


        #endregion

        #region Ctor

        public AControl(Region region)
        {            
            this.Region = region;
            //rendertemplate ?
        }


        #endregion

        #region Public Members

        public void AddTransformation(ITransformation transformation)
        {
            transformations.Add(transformation);
        }
        
        public virtual void Update(double deltaTime)
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

        public virtual void Draw(ABatchDrawable manager)
        {
            manager.Draw(this);
        }

        #endregion
      

    }
}
