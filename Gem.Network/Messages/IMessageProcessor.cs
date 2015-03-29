namespace Gem.Network.Messages
{
    /// <summary>
    /// Base interface for network incoming message processing
    /// </summary>
    internal interface IMessageProcessor 
    {
        void ProcessNetworkMessages();
    }
}
