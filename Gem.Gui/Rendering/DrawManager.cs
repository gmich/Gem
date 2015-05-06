using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Rendering
{
    public class DrawManager
    {
        private readonly AGuiRenderer renderer;
        private readonly GraphicsDevice device;
        private readonly AGuiRenderer drawManager;

        public GraphicsDevice Device { get { return device; } }
    }
}
