namespace Gem.Network.Configuration
{
    /// <summary>
    /// Returns the <see cref="GemConfiguration"></see>
    /// </summary>
    internal interface IConfigurationReader
    {
        GemConfiguration Load(string path);
    }
}
