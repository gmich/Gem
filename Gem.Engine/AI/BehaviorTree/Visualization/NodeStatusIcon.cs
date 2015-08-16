using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.AI.BehaviorTree.Visualization
{
    internal class NodeStatusIcon : IVirtualizationItem
    {
        private readonly double timeUntilDisposal;
        private readonly Func<Vector2> position;
        private double elapsedTime;

        public NodeStatusIcon(Texture2D texture, RenderedNode node, Func<Vector2> position, double timeUntilDisposal)
        {
            Node = node;
            Texture = texture;
            this.position = position;
            this.timeUntilDisposal = timeUntilDisposal;
        }

        public bool IsActive => (elapsedTime <= timeUntilDisposal);

        public RenderedNode Node { get; }

        public Color Color
        {
            get { return Color.White * (float)(1 - (elapsedTime / timeUntilDisposal)); }
        }

        public Vector2 Position
        {
            get { return position(); }
        }

        public Texture2D Texture { get; }

        public void Reset()
        {
            elapsedTime = 0.0d;
        }

        public void Update(double timeDelta)
        {
            elapsedTime += timeDelta;
        }

        public void Draw(SpriteBatch batch, Vector2 position, int sizeX, int sizeY)
        {
            batch.Draw(Texture,
                  new Rectangle(
                      (int)position.X - Texture.Width / 2,
                      (int)position.Y - Texture.Height,
                      Texture.Width,
                      Texture.Height),
                  null,
                  Color,
                  0.0f,
                  Vector2.Zero,
                  SpriteEffects.None,
                  0.4f);
        }
    }
}
