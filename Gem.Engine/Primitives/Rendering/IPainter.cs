using System.Collections.Generic;

namespace Gem.Engine.Primitives
{
    public interface IPainter
    {
        List<IShape> Shapes { get; }
        void Paint(double timeDelta);
    }
}