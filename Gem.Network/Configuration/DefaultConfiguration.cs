namespace Gem.Network.Configuration
{
    /// <summary>
    /// Holds the default Gem.Network configuration
    /// </summary>
    public class DefaultConfiguration : IConfigurationReader
    {

        private readonly GemConfiguration dependArgs;

        internal DefaultConfiguration()
        {
            dependArgs = new GemConfiguration
            {
                Factory = "default",
                RuntimeBuilder = "default"
            };
        }

        public GemConfiguration Load(string path)
        {
            return dependArgs;
        }

    }
}