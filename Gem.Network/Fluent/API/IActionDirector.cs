using System;

namespace Gem.Network.Fluent
{

    public interface IActionDirector
    {
        void Do(Action<Lidgren.Network.NetIncomingMessage> action);

        IMessageFlowBuilder Send(params object[] arguments);
    }

}
