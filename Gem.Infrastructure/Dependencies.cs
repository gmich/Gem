using Autofac;
using Castle.DynamicProxy;
using System.Diagnostics;
using Gem.Infrastructure.Aspects;
using Gem.Infrastructure.Logging;

namespace Gem.Infrastructure
{
    public sealed class Dependencies
    {
        public static IContainer container;
        private const string Log4NetAppender = "DebugLogger";

        static Dependencies()
        {
            var builder = new ContainerBuilder();

            RegisterInterceptors(builder);

            container = builder.Build();
        }

        private static void RegisterInterceptors(ContainerBuilder builder)
        {
            builder.Register(c => new LogInterceptor(new Log4NetWrapper(Log4NetAppender)))
                  .Named<IInterceptor>("Log");

            builder.Register(c => new LogicalOperationInterceptor())
                   .Named<IInterceptor>("LogicalOperation");

            builder.Register(c => new ExceptionInterceptor(new Log4NetWrapper(Log4NetAppender), ex => ex.Message))
                   .Named<IInterceptor>("Try");
        }
    }
}