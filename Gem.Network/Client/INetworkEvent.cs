using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.ClientEvents
{
    public interface INetworkEvent
    {
        void SubscribeEvent(IClient client);

        void Send(params object[] networkargs);

        void OnEvent(object message);
    }
}
