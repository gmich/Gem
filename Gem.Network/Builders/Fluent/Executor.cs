using System;
using System.Linq.Expressions;

namespace Gem.Network.Configuration
{

    public class Executor : ABuilder
    {
        public Executor(ClientNetworkInfoBuilder builder)
            : base(builder)
        { }

        public void Execute<T>(T objectToHandle, Expression<Func<T, Delegate>> methodToHandle)
        {
            throw new NotImplementedException();
        }
    }   

}




