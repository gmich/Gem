using Gem.Gui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Fluent
{
    public static class ButtonBuilder
    {
        public static Button Text(this Button button,string text)
        {
            return button;
        }

        public static Button TextColor(this Button button, string text)
        {
            return button;
        }
    }
}
