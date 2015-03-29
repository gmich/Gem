namespace Gem.Network.Events
{
    /// <summary>
    /// The base interface for outgoing messages that are serialized and then handled
    /// </summary>
    public interface INetworkPackage
    {
        byte Id { get; set; }
    }
}
