using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Engine.AI.BehaviorTree.Visualization
{
    internal class TriggeredNodeCover : IVirtualizationItem
    {
        private readonly double timeUntilDisposal;
        private readonly Color color;
        private readonly Func<Vector2> position;
        private double elapsedTime;

        public TriggeredNodeCover(Texture2D texture, RenderedNode node, Func<Vector2> position, double timeUntilDisposal, Color color)
        {
            Texture = texture;
            Node = node;
            this.position = position;
            this.timeUntilDisposal = timeUntilDisposal;
            this.color = color;
        }

        public bool IsActive => (elapsedTime <= timeUntilDisposal);

        public RenderedNode Node { get; }

        public Texture2D Texture { get; }

        public Color Color
        {
            get { return color * (float)(1 - (elapsedTime / timeUntilDisposal)); }
        }

        public Vector2 Position
        {
            get { return position(); }
        }

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
                    (int)position.X - sizeX / 2,
                    (int)position.Y,
                    sizeX,
                    sizeY),
                null,
                Color,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.25f);

        }

    }
}
