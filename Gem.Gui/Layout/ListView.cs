using Gem.Gui.Alignment;
using Gem.Gui.Configuration;
using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using Gem.Gui.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gem.Gui.Layout
{
    [Layout("ListView")]
    public class ListView : AControl
    {

        #region Fields

        private readonly ReadOnlyCollection<AControl> controls;
        private readonly IList<IDisposable> activeAlignments = new List<IDisposable>();
        private readonly Orientation orientation;
        private readonly Action<Region> AlignControls;

        #endregion

        #region Ctor

        public ListView(Texture2D texture,
                        AlignmentContext alignment,
                        Orientation orientation,
                        Region region,
                        ReadOnlyCollection<AControl> controls)
            : base(texture, region, new NoStyle())
        {
            this.ControlAlignment = alignment;
            this.controls = controls;
            this.orientation = orientation;

            if (orientation == Orientation.LandScape)
            {
                AlignControls = AlignAsLandscape;
            }
            else
            {
                AlignControls = AlignAsPortrait;
            }

            this.Align(Gem.Gui.Configuration.Settings.ViewRegion);
            this.Options.IsFocusEnabled = false;
        }


        #endregion

        #region ListView Members and Properties

        private void Flush()
        {
            foreach (var transformation in activeAlignments)
            {
                transformation.Dispose();
            }
        }

        public Orientation Orientation { get { return orientation; } }

        #endregion

        #region Alignment

        public AlignmentContext ControlAlignment { get; private set; }

        public override void Align(Region viewPort)
        {
            Flush();

            AlignControls(viewPort);
            base.Align(viewPort);            
        }

        public void AlignAsLandscape(Region viewPort)
        {
            throw new NotImplementedException();
        }

        public void AlignAsPortrait(Region viewPort)
        {
            Region offSetRegion = Region.Empty;
            
            foreach (var child in controls)
            {
                var parentRegion = new Region(this.Region.Position,
                                              new Vector2(this.Region.Size.X, child.Region.Size.Y)) + offSetRegion;
                var targetRegion = ControlAlignment.GetTargetRegion(parentRegion,
                                                                       child.Region,
                                                                       Padding.Zero);              
                targetRegion += new Region(0, child.Padding.Top, 0, 0);
                
                this.activeAlignments.Add(child.AddTransformation(
                                             ControlAlignment.Transition.CreateTransition(child.Region, 
                                                                                   targetRegion)));
                offSetRegion = new Region(0,
                                          (targetRegion.Position.Y - this.Region.Position.Y) + child.Region.Size.Y + child.Padding.Bottom,
                                          0,
                                          0);

            }
        }

        #endregion

        #region AControl Members
        
        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            foreach (var control in controls)
            {
                control.Update(deltaTime);
            }
        }

        public override void Render(SpriteBatch batch, RenderTemplate template)
        {
            base.Render(batch, template);

            foreach (var control in controls)
            {
                control.Render(batch, template);
            }
        }

        public override IEnumerable<AControl> Entries()
        {
            foreach (var control in controls)
            {
                yield return control;
            }
        }

        #endregion
    }
}
