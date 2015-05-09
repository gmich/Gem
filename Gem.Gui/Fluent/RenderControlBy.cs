using Gem.Gui.Rendering;
using System;

namespace Gem.Gui.Fluent
{
    public sealed class RenderControlBy
    {
        private static Lazy<ControlPositionDrawable> _position = new Lazy<ControlPositionDrawable>();
        public static IControlDrawable Position
        {
            get { return _position.Value; }
        }

        private static Lazy<ControlFrameDrawable> _frame = new Lazy<ControlFrameDrawable>();
        public static IControlDrawable Frame
        {
            get { return _frame.Value; }
        }

    }
}
