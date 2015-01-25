using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network
{
    public interface IDebugListener
    {
        event Action<string> WriteMessage;
    }
}
