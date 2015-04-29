using System;

namespace Gem.Gui.Controls
{
    [Flags]
    public enum ControlTarget
    {
        None = 0,
        Mouse = 1,
        Keyboard = 2,
        Touch = 4,
        Controller = 8
    }
}
