using Microsoft.Xna.Framework;

namespace Gem.Engine.AI.Voronoi
{
    /// <summary>
    /// A class that stores an edge in Voronoi diagram
    /// </summary>
    public class VoronoiEdge
    {

        public VoronoiEdge(Vector2 start, Vector2 left, Vector2 right)
        {
            Start = start;
            Right = right;
            Left = left;
            F = (right.X - left.X) / (left.Y - right.Y);
            G = start.Y - F * start.X;
            Direction = new Vector2(right.Y - left.Y, -(right.X - left.X));
        }

        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        /// <summary>
        /// Directional vector, from "start", points to "end", normal of |left, right|
        /// </summary>
        public Vector2 Direction { get; }

        /// <summary>
        /// Voronoi place on the left side of the edge
        /// </summary>
        public Vector2 Left { get; }

        /// <summary>
        /// Voronoi place on the right side of the edge
        /// </summary>
        public Vector2 Right { get; }

        /// <summary>
        /// directional coeffitients satisfying equation y = f*x + g (edge lies on this line)
        /// </summary>
        public double F { get; }
        public double G { get; }

        /// <summary>
        /// Some edges consist of two parts
        /// </summary>
        public VoronoiEdge Neighbour { get; set; }

    }
}
