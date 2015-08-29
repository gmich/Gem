using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.AI.FiniteStateMachine.Visualization
{
    class Vertex : IVertex, IEquatable<IVertex>
    {
        public Vertex(int reference)
        {
            NodeReference = reference;
        }

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

        public bool Equals(IVertex other)
        {
            return NodeReference == other.NodeReference;
        }

        public static bool operator ==(Vertex a, IVertex b)
        {
            return (a.NodeReference == b.NodeReference);
        }

        public static bool operator !=(Vertex a, IVertex b)
        {
            return (a.NodeReference != b.NodeReference);
        }

        public override int GetHashCode()
        {
            return NodeReference.GetHashCode();
        }
    }
}
