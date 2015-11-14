using Microsoft.Xna.Framework;

namespace Gem.Engine.AI.Voronoi
{
    public class VoronoiParabola
    {
        public bool IsLeaf;
        public Vector2 Site;
        public VoronoiEdge Edge;
        public VoronoiEvent CEvent;
        public VoronoiParabola Parent;

        public VoronoiParabola Left { get; set; }
        public VoronoiParabola Right { get; set; }

        public VoronoiParabola(Vector2 site)
        {
            Site = site;
            IsLeaf = true;
        }
        public VoronoiParabola()
        { }

        public VoronoiParabola GetLeft(VoronoiParabola other)
        {
            return GetLeftChild(GetLeftParent(other));
        }


        public VoronoiParabola GetRight(VoronoiParabola other)
        {
            return GetRightChild(GetRightParent(other));
        }

        public static VoronoiParabola GetLeftParent(VoronoiParabola other)
        {
            VoronoiParabola par = other.Parent;
            VoronoiParabola pLast = other;
            while (par.Left == pLast)
            {
                if (par.Parent == null) return null;
                pLast = par;
                par = par.Parent;
            }
            return par;
        }

        public static VoronoiParabola GetRightParent(VoronoiParabola other)
        {
            VoronoiParabola par = other.Parent;
            VoronoiParabola pLast = other;
            while (par.Right == pLast)
            {
                if (par.Parent == null) return null;
                pLast = par; par = par.Parent;
            }
            return par;
        }

        public static VoronoiParabola GetLeftChild(VoronoiParabola other)
        {
            if (other == null) return null;
            VoronoiParabola par = other.Left;
            while (par.IsLeaf) par = par.Right;
            return par;
        }

        public static VoronoiParabola GetRightChild(VoronoiParabola other)
        {
            if (other == null) return null;
            VoronoiParabola par = other.Right;
            while (!par.IsLeaf) par = par.Left;
            return par;
        }

    }
}
