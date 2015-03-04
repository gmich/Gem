using Lidgren.Network;
using System;

namespace Gem.Network.Events
{
    public interface INetworkPackage
    {
        byte Id { get; set; }

    }
}
