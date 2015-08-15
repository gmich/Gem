using Microsoft.Xna.Framework;
using System;

namespace Gem.AI.BehaviorTree.Visualization
{
    public class LinkBase : IBehaviorVirtualizationPiece
    {
        private readonly Func<float> position;

        public LinkBase(RenderedNode linkedNode, Func<float> position, int nodeCount)
        {
            NodeCount = nodeCount;
            LinkedNode = linkedNode;
            this.position = position;
        }

        public float PositionX
        {
            get
            {
                return position();
            }
        }


        public Color Color { get; set; }

        public RenderedNode LinkedNode { get; }

        public int NodeCount { get; }
    }
}
