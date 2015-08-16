using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Gem.Engine.BehaviorTreeVisualization
{
    internal class Key : IGameComponent
    {
        private readonly Texture2D texture;
        private readonly int width;
        private readonly int height;
        public Key(Texture2D texture, int width, int height)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
        }

        private int offSetX => (width / 2 - texture.Width / 2);
        private int offSetY => (height / 2 + texture.Height /2 );
        public void Draw(SpriteBatch batch, Vector2 position)
        {
            batch.Draw(texture, new Rectangle((int)position.X + offSetX, (int)position.Y + offSetY, texture.Width,texture.Height), Color.White);
        }
    }
}
