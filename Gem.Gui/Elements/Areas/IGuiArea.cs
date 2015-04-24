using Gem.Gui.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Elements
{
    public interface IGuiArea : IGuiComponent
    {
        int ComponentCount { get; }

        int AddComponent(IGuiComponent component);

        bool RemoveComponent(int id);

        IEnumerable<IGuiComponent> Components();

        IGuiComponent this[int id] { get; }
    }
}
