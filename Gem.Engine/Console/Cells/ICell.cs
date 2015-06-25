using Gem.Console.Animations;
using Gem.Infrastructure.Functional;

namespace Gem.Console
{
    public interface ICell
    {
        string Content { get; }

        int SizeX { get; }

        int SizeY { get; }

        Behavior<IEffect> Behavior { get; set; }
    }
}
