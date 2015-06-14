using Gem.Infrastructure.Functional;

namespace Gem.Console
{
    public interface ICell
    {
        string Content { get; }

        int SizeX { get; }

        int SizeY { get; }

    }
}
