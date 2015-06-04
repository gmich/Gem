using System;
namespace Gem.Console.Rendering
{
    public class FixedSizeArea : IRenderArea
    {

        private readonly RenderRegion region;

        public FixedSizeArea(RenderRegion region, Func<RenderRegion> translator)
        {
            this.region = region;
        }

        public RenderRegion Region
        {
            get { return region; }
        }
    
    }
}