using Microsoft.Xna.Framework;
using System;


namespace Gem.AI.BehaviorTree.Visualization
{

    public class RenderedNode : IBehaviorVirtualizationPiece
    {
        private readonly Func<float> getPosition;

        public RenderedNode(string type, string name, Func<float> getPosition)
        {
            Name = name;
            Type = type;
            this.getPosition = getPosition;
        }

        public float OffsetX { get; set; } = 0.0f;

        public string Name { get; }

        public string Type { get; }

        public float PositionX { get { return getPosition() + OffsetX; } }

        public Color Color { get; set; }
    }
}
