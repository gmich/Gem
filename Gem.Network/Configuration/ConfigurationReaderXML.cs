using Gem.Network.Utilities.Loggers;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Gem.Network.Configuration
{
    public class ConfigurationReaderXML : IConfigurationReader
    {
        private DependencyArgs dependArgs;

        #region Dependency Reader

        public void Load(string path)
        {
            XDocument doc = new XDocument();

            try
            {
                this.dependArgs =
                XDocument.Load(path)
                         .Element("Gem")
                         .Descendants("Network")
                         .Select(x => new DependencyArgs
                             {
                                 Factory = x.Element("Factory").Value,
                                 RuntimeBuilder = x.Element("RuntimeBuilder").Value
                             })
                         .Single();

                GemNetworkDebugger.Append.Info("Network configuration loaded successfully");
            }
            catch (Exception ex)
            {
                GemNetworkDebugger.Append.Error(@"Failed to load Network configuration.
                                      Falling back to the default settings. Reason: {0}", ex.Message);

                dependArgs = new DependencyArgs { Factory = "default", RuntimeBuilder = "default" };
            }
        }

        public DependencyArgs Dependencies
        {
            get { return dependArgs; }
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
