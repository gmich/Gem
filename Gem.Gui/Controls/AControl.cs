using Gem.Gui.Configuration;
using Gem.Gui.Core.Controls;
using Gem.Gui.Core.Styles;
using Gem.Gui.Events;
using Gem.Gui.Rendering;
using Gem.Gui.Text;
using Gem.Gui.Transformations;
using Microsoft.Xna.Framework.Graphics;
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

        private readonly IList<ITransformation> transformations = new List<ITransformation>();

        #endregion

        #region Properties

        public ViewEvents<ControlEventArgs> Events { get; set; }

        public RenderParameters RenderParameters { get; set; }

        public Sprite Sprite { get; set; }

        public IRenderStyle RenderStyle { get; set; }

        public IText Text { get; set; }

        public Options Options { get; set; }

        public Region Region { get; set; }

        public Padding Padding { get; set; }

        #endregion

        #region Ctor

        public AControl(Texture2D texture, Region region)
        {
            this.Region = region;
            this.Sprite = new Sprite(texture);
            this.RenderStyle = new PlainControlStyle(this);
            this.RenderParameters = new RenderParameters();
            this.Options = new Options();
            this.Events = new ViewEvents<ControlEventArgs>(this, () => new ControlEventArgs());
        }

        #endregion

        #region Public Members

        public virtual void Align(Region viewPort)
        {
            if(Text!=null)
            {
                foreach(var transformation in Text.Alignment.AlignementTransformations(this.Region,Text.Region,Text.Padding))
                {
                    Text.Alignment.ActiveTransformations(this.AddTransformation(transformation));
                }
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
                if (transformations[index].Enabled)
                {
                    transformations.RemoveAt(index);
                    continue;
                }
                transformations[index].Transform(this, deltaTime);
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
