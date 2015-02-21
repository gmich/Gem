using Lidgren.Network;

namespace Gem.Network.Messages
{

    public interface IServerMessage
    {

        #region Public Properties

        MessageType MessageType { get; }

        #endregion


        #region Public Methods and Operators

        void Decode(NetIncomingMessage im);

        void Encode(NetOutgoingMessage om);

        #endregion
    }
}