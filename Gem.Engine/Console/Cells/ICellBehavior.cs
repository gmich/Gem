using Gem.Engine.Console.Rendering.Animations;
using Gem.Infrastructure.Functional;

namespace Gem.Engine.Console.Cells
{
    public interface ICellBehavior
    {
        Behavior<IEffect> CreateEffect(ICell cell);
    }
}
