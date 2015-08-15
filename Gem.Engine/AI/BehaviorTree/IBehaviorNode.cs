using System;
using System.Collections.Generic;

namespace Gem.AI.BehaviorTree
{
    public interface IBehaviorNode<AIContext>
    {
        event EventHandler OnBehaved;

        BehaviorResult Behave(AIContext context);

        IEnumerable<IBehaviorNode<AIContext>> SubNodes { get; }
    }
}
