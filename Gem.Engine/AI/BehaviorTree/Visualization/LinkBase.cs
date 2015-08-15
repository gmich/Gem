using Microsoft.Xna.Framework;
using System;

namespace Gem.AI.BehaviorTree.Visualization
{
    public class LinkBase : IBehaviorVirtualizationPiece
    {
        private readonly Func<float> center;

        public LinkBase(Func<float> center, int nodeCount, int linkSize)
        {
            this.NodeCount = nodeCount;
            this.center = center;
            PositionX = Center - linkSize / 2;
        }

        public float PositionX { get; }

        public Color Color { get; set; }

        public float Center
        {
            get { return center(); }
        }

        public int NodeCount { get; }
    }
}
