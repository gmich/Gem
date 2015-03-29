using Gem.Network.Events;
using Gem.Network.Messages;
using Lidgren.Network;

namespace Gem.Network.Messages
{
    /// <summary>
    /// This is used to notify a server or a client
    /// </summary>
    public partial class Notification 
    {
        public Notification(NetIncomingMessage im)
        {
            Message = im.ReadString();
            Type = im.ReadString();
        }

        public Notification(string message,string type)
        {
            this.Message = message;
            this.Type = type;
        }

        private readonly byte id = GemNetwork.NotificationByte;
        public byte Id
        {
            get
            {
                return id;
            }
        }

        public string Message { get; set; }

        public string Type { get; set; }
    }
 
    public static class NotificationType
    {
        public const string Message = "#Message";
        public const string Command = "#Command";
    }
}