using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.AI.FiniteStateMachine.Visualization
{
    class Edge
    { 
        public Edge(Vertex source, Vertex target, Direction direction = Direction.SourceToDestination)
        {
            Target = target;
            Source = source;
            Direction = direction;
        }

        public Vertex Target
        {
            get;
        }

        public Direction Direction
        {
            get;
            set;
        }

        public Vertex Source
        {
            get;
        }
    }
}
