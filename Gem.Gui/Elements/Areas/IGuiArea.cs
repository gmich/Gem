using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Elements
{
    interface IGuiArea : IGuiElement
    {
        int AddElement(IGuiElement element);

        bool RemoveElement(int id);

        IEnumerable<IGuiElement> Elements();

        IGuiElement this[int id] { get; }
    }
}
