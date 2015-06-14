using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Console.Animations
{
    public class DrawEffect : IEffect
    {
        private readonly Action<SpriteFont, SpriteBatch, Vector2> drawAction;

        public DrawEffect(Action<SpriteFont, SpriteBatch, Vector2> drawAction)
        {
            this.drawAction = drawAction;
        }
        public Action<SpriteFont, SpriteBatch, Vector2> DrawAction { get { return drawAction; } }

        public void Draw(SpriteFont font, SpriteBatch batch, Vector2 location)
        {
            DrawAction(font, batch, location);
        }
    }
}
