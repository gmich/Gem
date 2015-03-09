using Gem.Network.Messages;
using Lidgren.Network;
using System;

namespace Gem.Network.Other
{

    /// <summary>
    /// Indicates a class that's marked for network serialization and delivery
    /// NetworkPackageAttribute marked class must always implement INetworkPackage interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class NetworkPackageAttribute : Attribute
    {
        //TODO: implement
    }
      
}