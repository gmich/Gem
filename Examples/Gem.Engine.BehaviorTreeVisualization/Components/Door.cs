using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Gem.Engine.BehaviorTreeVisualization
{
    internal class Door : IGameComponent
    {
        private readonly Texture2D texture;
        private readonly int width;
        private readonly int height;
        public Door(Texture2D texture, int width, int height)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
        }
        public void Draw(SpriteBatch batch, Vector2 position)
        {
            batch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);
        }
    }
}
