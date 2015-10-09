using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Engine.Console.Cells
{
    public class Cell : ICell
    {
        public char Content { get; }
        private Action<SpriteBatch, Vector2> renderAction;

        public Cell(char content, Action<SpriteBatch, Vector2> renderAction)
        {
            this.Content = content;
            this.renderAction = renderAction;
        }


        public void Render(SpriteBatch batch, Vector2 position)
        {
            renderAction(batch, position);
        }
    }
}
