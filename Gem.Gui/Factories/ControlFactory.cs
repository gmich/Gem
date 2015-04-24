using Gem.Gui.Aggregators;
using Gem.Gui.Controls;
using Gem.Gui.Controls.Aggregators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Factories
{
    public static class ControlFactory
    {
        public static IControlAggregator CreateControl<TEventArgs>(ControlTarget target, Control<TEventArgs> control)
            where TEventArgs : EventArgs
        {

            switch(target)
            {

            }
            return null;
        }
    }
}
