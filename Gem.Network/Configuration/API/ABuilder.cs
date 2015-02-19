namespace Gem.Network.Configuration
{
    public abstract class ABuilder
    {
        protected static int profilesCalled = 0;
        protected readonly ClientNetworkInfoBuilder builder;

        public ABuilder(ClientNetworkInfoBuilder builder)
        {
            this.builder = builder;
        }
    }
}




