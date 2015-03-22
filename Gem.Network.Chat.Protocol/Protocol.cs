using Gem.Network.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Chat.Protocol
{
    [NetworkPackage("GemChat")]
    public class Package
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
