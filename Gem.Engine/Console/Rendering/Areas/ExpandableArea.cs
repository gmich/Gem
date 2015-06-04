using Microsoft.Xna.Framework;
namespace Gem.Console.Rendering.Areas
{
    class ExpandableArea : IRenderArea
    {

        private readonly FixedSizeArea area;
        private Vector2 position;


        public ExpandableArea(FixedSizeArea area)
        {
            this.area = area;
        }

        public Rectangle Region
        {
            get;
            private set;
        }

        public RenderRegion VirtualRegion
        {
            get;
            internal set;
        }

        public Vector2 Position
        {
            get { return position; }
            set 
            {
                position = value;
                //RenderArea = new Rectangle(position.X,position.Y,vi)
            }
        }
        public RenderRegion ViewPort
        {
            get;
            internal set;
        }
        
        RenderRegion IRenderArea.Region
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
