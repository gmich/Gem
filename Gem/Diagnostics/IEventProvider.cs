using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gem.Diagnostics
{

    public interface IEventProvider<Targs>
        where Targs : EventArgs
    {

        event EventHandler<Targs> OnStartEvent;

        event EventHandler<Targs> OnProgressEvent;

        event EventHandler<Targs> OnEndEvent;

    }
    
}
