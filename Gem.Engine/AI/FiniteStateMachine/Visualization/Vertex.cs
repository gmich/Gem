using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.AI.FiniteStateMachine.Visualization
{
    class Vertex : IEquatable<Vertex>
    {
        public Vertex(int reference)
        {
            NodeReference = reference;
        }

        private static Vertex placeHolder = new Vertex(-1);
        public static Vertex PlaceHolder
        { get { return placeHolder; } }

        public int NodeReference { get; }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
            {
                return false;
            }
            if (ReferenceEquals(this, right))
            {
                return true;
            }
            if (GetType() != right.GetType())
            {
                return false;
            }
            return Equals(right as Vertex);
        }

        public bool Equals(Vertex other)
        {
            return NodeReference == other.NodeReference;
        }

        public static bool operator ==(Vertex a, Vertex b)
        {
            return (a.NodeReference == b.NodeReference);
        }

        public static bool operator !=(Vertex a, Vertex b)
        {
            return (a.NodeReference != b.NodeReference);
        }

        public override int GetHashCode()
        {
            return NodeReference.GetHashCode();
        }
    }
}
