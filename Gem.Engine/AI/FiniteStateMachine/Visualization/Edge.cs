using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.AI.FiniteStateMachine.Visualization
{
    class Edge : IEdge
    {
        public Edge(IVertex source, IVertex target, Direction direction = Direction.SourceToDestination)
        {
            Target = target;
            Source = source;
            Direction = direction;
        }

        public IVertex Target
        {
            get;
        }

        public Direction Direction
        {
            get;
            set;
        }

        public IVertex Source
        {
            get;
        }
    }
}
