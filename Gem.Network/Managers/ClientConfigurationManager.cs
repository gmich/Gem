using Gem.Network.Messages;
using System;

namespace Gem.Network.Managers
{
    public class ClientConfigurationManager
    {

        public Func<ConnectionApprovalMessage> ApprovalMessageDelegate { get; set; }

    }
}
