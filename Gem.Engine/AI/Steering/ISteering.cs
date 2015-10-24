using System;

namespace Gem.Engine.AI.Steering
{
    public interface ISteering
    {
        IDisposable AddAgent(Agent agent);

        //Called once per initialization
        void Assign();
    }
}
