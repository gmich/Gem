using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Gem.Engine.BehaviorTreeVisualization
{
    internal interface IGameComponent
    {
        void Draw(SpriteBatch batch, Vector2 position);
    }
}
