using System;

namespace Gem.Engine.AI.FSM
{
    public class FSMException : Exception
    {
        public FSMException(string message) : base(message)
        {
        }
    }
}
