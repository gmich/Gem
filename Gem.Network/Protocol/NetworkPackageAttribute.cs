using Gem.Network.Messages;
using Lidgren.Network;
using System;

namespace Gem.Network.Protocol
{
    /// <summary>
    /// Indicates a class that's marked for network serialization and delivery
    /// Classes of that are annoted with NetworkPackageAttribute can be handled
    /// by the client and the server
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