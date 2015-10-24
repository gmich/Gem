using Microsoft.Xna.Framework;
using System;


namespace Gem.Engine.AI.BehaviorTree.Visualization
{

    public class RenderedNode : IBehaviorVirtualizationPiece
    {
        private readonly Func<float> getPosition;
        public EventHandler<EventArgs> onTriggered;

        public RenderedNode(string behaviorType,int row, string name, Func<float> getPosition)
        {
            Name = name;
            BehaviorType = behaviorType;
            this.getPosition = getPosition;
            Row = row;
        }

        public BehaviorResult? BehaviorStatus { get; set; } = null;
        
        public float OffsetX { get; set; } = 0.0f;

        public string Name { get; }

        public string BehaviorType { get; }

        public float PositionX { get { return getPosition() + OffsetX; } }
        
        public int Row { get; }
        public void Trigger()
        {
            onTriggered?.Invoke(this, EventArgs.Empty);
        }

    }
}
