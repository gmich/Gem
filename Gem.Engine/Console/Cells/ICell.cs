using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Engine.Console.Cells
{
    public interface ICell
    {
        char Content { get; }

        void Render(SpriteBatch batch, Vector2 position);
    }
}
