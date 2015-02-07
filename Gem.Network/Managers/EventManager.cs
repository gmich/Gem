using Gem.Network.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Managers
{
    class EventManager
    {
        public void Register(Type poco)
        {
            var eventRaisingType = EventBuilder.Create(poco);
            //eventRaisingclass = Activator.CreateInstance(eventRaisingType);
        }
    }
}
