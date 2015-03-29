using Autofac;
using Castle.DynamicProxy;
using Gem.Network.Builders;
using Gem.Network.Configuration;
using Gem.Network.Factories;
using Gem.Network.Utilities.Loggers;

namespace Gem.Network
{
    internal sealed class Dependencies
    {
        internal static IContainer Container { get; private set; }

        internal static void Setup(GemConfiguration dependencies)
        {
            var builder = new ContainerBuilder();

            RegisterFactories(builder, dependencies.Factory);

            RegisterBuilders(builder, dependencies.RuntimeBuilder);

            RegisterInterceptors(builder);

            Dependencies.Container = builder.Build();
        }

        private static void RegisterInterceptors(ContainerBuilder builder)
        {
            builder.Register(c => new LogInterceptor(
                                  new ActionAppender(GemNetworkDebugger.Echo)))
                            .Named<IInterceptor>("Log");
        }

        private static void RegisterFactories(ContainerBuilder builder, string factory)
        {
            switch (factory)
            {
                default:
                    builder.RegisterType<ClientEventFactory>().As<IEventFactory>().SingleInstance();

                    builder.Register(c => new MessageHandlerFactory(c.Resolve<IMessageHandlerBuilder>()))
                           .As<IMessageHandlerFactory>().SingleInstance();

                    builder.Register(c => new PocoTypeFactory(c.Resolve<IPocoBuilder>()))
                                    .As<IPocoFactory>().SingleInstance();
                    break;
            }
        }

        private static void RegisterBuilders(ContainerBuilder builder, string runtimeBuilder)
        {
            switch (runtimeBuilder)
            {
                default:
                    builder.RegisterType<CsScriptPocoBuilder>()
                           .As<IPocoBuilder>().SingleInstance();

                    builder.RegisterType<CsScriptBuilder>()
                           .As<IMessageHandlerBuilder>().SingleInstance();
                    break;
            }
        }
    }
}
