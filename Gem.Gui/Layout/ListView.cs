using Gem.Gui.Alignment;
using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using System.Linq;

namespace Gem.Gui.Layout
{
    public enum Orientation
    {
        Portrait,
        LandScape
    }

    public class ListView : AControl
    {
        private readonly ReadOnlyCollection<AControl> controls;
        private readonly Orientation orientation;
        private readonly AlignmentContext alignment;

        public ListView(Texture2D texture,
                        AlignmentContext alignment,
                        Orientation orientation,
                        Region region,
                        ReadOnlyCollection<AControl> controls)
            : base(texture, region)
        {
            this.alignment = alignment;
            this.controls = controls;
            this.orientation = orientation;
        }

        public override void Align(Region viewPort)
        {
            //portrait
            base.Align(viewPort);

            //foreach (var transformation in alignment.AlignementTransformations(this.Region, viewPort, Padding))
            {
                //TODO: use IDisposable to dispose active transformations
            }

            int controlCount = controls.Count;
            float controlSizeX = controls.Sum(control => control.Region.Size.X);
            float controlSizeY = controls.Sum(control => control.Region.Size.Y);

            //TODO: implement
        }
    }
}
