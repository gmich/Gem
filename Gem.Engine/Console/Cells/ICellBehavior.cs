using Gem.Console.Animations;
using Gem.Infrastructure.Functional;

namespace Gem.Console
{
    public interface ICellBehavior
    {
        Behavior<IEffect> CreateEffect(ICell cell);
    }
}
