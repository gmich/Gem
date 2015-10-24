using System;
using System.Collections.Generic;

namespace Gem.Engine.AI.BehaviorTree
{
    public interface IBehaviorNode<AIContext>
    {
        event EventHandler OnBehaved;

        string Name { get; set; }
        BehaviorResult Behave(AIContext context);

        IEnumerable<IBehaviorNode<AIContext>> SubNodes { get; }

    }
}
