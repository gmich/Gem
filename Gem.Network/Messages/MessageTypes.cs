namespace Gem.Network.Messages
{
    /// <summary>
    /// Outgoing / Incoming message types
    /// </summary>
    public enum MessageType
    {
        ServerNotification,

        Handshake,

        Connecting,

        Connected,

        Disconnecting,

        Disconnected,

        DiscoveryRequest,

        DiscoveryResponse,

        Data,

        Warning,

        Error
    }
    
}


