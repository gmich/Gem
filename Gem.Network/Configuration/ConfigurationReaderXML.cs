using Gem.Network.Utilities.Loggers;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Gem.Network.Configuration
{
    /// <summary>
    /// Loads Gem.Network configuration by xml
    /// Format:
    /// <Gem>
    ///     <Network>
    ///              <Factory> "factory type"</Factory>
    ///              <RuntimeBuilder> "runtime builder"</RuntimeBuilder>
    ///     </Network>
    /// </Gem>
    /// </summary>
    public class ConfigurationReaderXML : IConfigurationReader
    {

        #region Dependency Reader

        public GemConfiguration Load(string path)
        {
            XDocument doc = new XDocument();

            try
            {
                var gemConfig =
                XDocument.Load(path)
                         .Element("Gem")
                         .Descendants("Network")
                         .Select(x => new GemConfiguration
                             {
                                 Factory = x.Element("Factory").Value,
                                 RuntimeBuilder = x.Element("RuntimeBuilder").Value
                             })
                         .Single();

                GemNetworkDebugger.Append.Info("Network configuration loaded successfully");
                return gemConfig;
            }
            catch (Exception ex)
            {
                GemNetworkDebugger.Append.Error(@"Failed to load Network configuration.
                                      Falling back to the default settings. Reason: {0}", ex.Message);

                return new GemConfiguration { Factory = "default", RuntimeBuilder = "default" };
            }
        }

        #endregion
        
        #region Private Helpers
        
        private object ConvertEnum<T>(string param)
            where T : struct
        {
            return Enum.Parse(typeof(T), param);
        }

        #endregion

    }
}
