using Gem.Network.Messages;
using Lidgren.Network;
using System;

namespace Gem.Network.Protocol
{

    public interface INetworkProtocol { }

    /// <summary>
    /// Indicates a class that's marked for network serialization and delivery
    /// NetworkPackageAttribute marked class must always implement INetworkPackage interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class NetworkPackageAttribute : Attribute
    {
        public string Profile { get; private set; }

        public byte Id { get; set; }

        public NetworkPackageAttribute(string profile)
        {
            this.Profile = profile;
        }
    }
}