using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gem.Network.Events
{

    public interface IEventProvider<Targs>
        where Targs : EventArgs
    {

        event EventHandler<Targs> OnAddEvent;

        event EventHandler<Targs> OnUsedMemoryEvent;

    }
    
}
