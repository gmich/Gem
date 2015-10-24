using Gem.Engine.AI.BehaviorTree;
using System;

namespace Gem.Engine.BehaviorTreeVisualization.Behaviors
{
    internal class BehaviorContext
    {
        private IBehaviorNode<BehaviorContext> behavior;

        public event EventHandler onBehaviorChanged;

        public BehaviorContext(Level level)
        {
            Level = level;
        }
        public IBehaviorNode<BehaviorContext> Behavior

        {
            get { return behavior; }
            set
            {
                behavior = value;
                onBehaviorChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        

        public Level Level { get; }
    }
}
