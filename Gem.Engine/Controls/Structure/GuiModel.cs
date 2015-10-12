using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.Controls.Structure
{
    public class GuiModel
    {
        ITree<IGuiElement> Structure { get; }

        public GuiModel()
        {
            Structure = NodeTree<IGuiElement>.NewTree();
            
        }
    }
}
