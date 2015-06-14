using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Console.Animations
{
    public static class Effects
    {
        public static IEffect Cell(string text, Color color, float transparency, float rotation, float scale, float layer, SpriteEffects spriteEffect)
        {
            return new DrawEffect((font, batch, position) =>
                    batch.DrawString(font,
                                     text,
                                     position,
                                     color,
                                     rotation,
                                     Vector2.Zero,
                                     scale,
                                     spriteEffect,
                                     layer));
        }

        public static IEffect Translate(this IEffect effect, float x, float y)
        {
            return new DrawEffect((font, batch, pos) => effect.Draw(font, batch, new Vector2(x, y)));
        }

        public static IEffect Compose(this IEffect effect1, IEffect effect2)
        {
            return new DrawEffect((font, batch, pos) =>
            {
                effect1.Draw(font, batch, pos);
                effect2.Draw(font, batch, pos);
            });
        }
    }
}
