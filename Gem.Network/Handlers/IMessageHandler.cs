using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Handlers
{
    public interface IMessageHandler
    {
        void Handle(params object[] args);
    }
}
