using Gem.Engine.Console.Rendering.Animations;
using Gem.Infrastructure.Functional;

namespace Gem.Engine.Console.Cells
{
    public interface ICell
    {
        string Content { get; }

        int SizeX { get; }

        int SizeY { get; }

        Behavior<IEffect> Behavior { get; set; }
    }
}
