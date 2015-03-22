using Gem.Network.Events;
using Gem.Network.Server;
using System;
using System.Linq.Expressions;

namespace Gem.Network.Fluent
{

    public interface IMessageFlowBuilder
    {
        INetworkEvent AndHandleWith<T>(T objectToHandle, Expression<Func<T, Delegate>> methodToHandle);
    }

    public interface IProtocolMessageBuilder<Target>
        where Target: new()
    {
        IProtocolMessageBuilder<Target> HandleIncoming(Action<Target> action);
        INetworkEvent GenerateSendEvent();
    }

    public interface IServerProtocolMessageBuilder<Target>
    where Target : new()
    {
        IServerProtocolMessageBuilder<Target> HandleIncoming(Action<Target> action);
        IProtocolServerEvent GenerateSendEvent();
    }

}
