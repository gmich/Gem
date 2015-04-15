using Gem.Gui.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Layout
{
    public interface IAlignable
    {
        void Align(Region parent, Region region);
    }
}
