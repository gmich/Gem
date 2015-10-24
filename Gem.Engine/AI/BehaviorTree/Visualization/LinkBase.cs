using Microsoft.Xna.Framework;
using System;

namespace Gem.Engine.AI.BehaviorTree.Visualization
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
        
        public RenderedNode LinkedNode { get; }

        public int NodeCount { get; }
    }
}
