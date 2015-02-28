using Gem.Network.Events;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gem.Network.Fluent
{
    public interface IMessageFlowBuilder
    {
        INetworkEvent HandleWith<T>(T objectToHandle, Expression<Func<T, Delegate>> methodToHandle);
    }
}
