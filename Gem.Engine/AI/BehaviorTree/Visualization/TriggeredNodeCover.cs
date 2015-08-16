using Microsoft.Xna.Framework;
using System;

namespace Gem.AI.BehaviorTree.Visualization
{
    internal class TriggeredNodeCover
    {
        private readonly double timeUntilDisposal;
        private readonly Color color;
        private readonly Func<Vector2> position;
        private double elapsedTime;

        public TriggeredNodeCover(RenderedNode node, Func<Vector2> position, double timeUntilDisposal, Color color)
        {
            this.Node = node;
            this.position = position;
            this.timeUntilDisposal = timeUntilDisposal;
            this.color = color;
        }

        public bool IsActive => (elapsedTime <= timeUntilDisposal);

        public RenderedNode Node { get; }

        public Color Color
        {
            get { return color * (float)(1 - (elapsedTime/ timeUntilDisposal)); }
        }

        public Vector2 Position
        {
            get { return position(); }
        }

        public void Reset()
        {
            elapsedTime = 0.0d;
        }

        public void Update(double timeDelta)
        {
            elapsedTime += timeDelta;
        }

    }
}
