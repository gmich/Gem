using Gem.Network.Commands;
using Gem.Network.Messages;
using System;

namespace Gem.Network.Fluent
{

    public interface IClientMessageRouter
    {
        IMessageFlowBuilder Send(MessageType messageType);    
        
        IMessageFlowBuilder WhenReceived(MessageType messageType);
    }

}
