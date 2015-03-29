using Lidgren.Network;
namespace Gem.Network.Handlers
{
    public class DummyHandler : IMessageHandler
    {
        public void Handle(NetConnection sender, object args)
        { }
    }
}
