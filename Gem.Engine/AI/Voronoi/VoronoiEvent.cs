using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.AI.Voronoi
{
    public class VoronoiEvent : IComparable
    {
        public Vector2 Position { get; set; }
        public bool IsPlaceEvent { get; set; }
        public VoronoiParabola Arch { get; set; }

        public VoronoiEvent(Vector2 position, bool pev)
        {
            Position = position;
            IsPlaceEvent = pev;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            var other = obj as VoronoiEvent;
            if (other != null)
                return this.Position.Y.CompareTo(other.Position.Y);
            else
                throw new ArgumentException("Object is not a VoronoiEvent");
        }
    }

}
