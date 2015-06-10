using Gem.Console.Rendering;
using Gem.Infrastructure.Functional;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Console.Effects
{
    public interface IEffect
    {
        void Draw(SpriteFont font, SpriteBatch batch, Vector2 location);
    }

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

    public static class Animate
    {
        public static Behavior<IEffect> Compose(this Behavior<IEffect> effect1, Behavior<IEffect> effect2)
        {
            return Behavior.Lift<IEffect, IEffect, IEffect>(Effects.Compose)(effect1, effect2);
        }

        public static Behavior<IEffect> Cell(Behavior<string> text, Behavior<Color> color, Behavior<float> alphaChannel)
        {
            return Cell(text, color, alphaChannel, 0.0f.Forever(), 1.0f.Forever(), 0.0f.Forever(), Behavior.Create(ctx => SpriteEffects.None));
        }

        public static Behavior<IEffect> Cell(Behavior<string> text, Behavior<Color> color, Behavior<float> alphaChannel, Behavior<float> rotation)
        {
            return Cell(text, color, alphaChannel, rotation, 1.0f.Forever(), 0.0f.Forever(), Behavior.Create(ctx => SpriteEffects.None));
        }

        public static Behavior<IEffect> Cell(Behavior<string> text, Behavior<Color> color, Behavior<float> alphaChannel, Behavior<float> rotation, Behavior<float> scale, Behavior<float> layer, Behavior<SpriteEffects> spriteEffect)
        {
            return Behavior.Lift<string, Color, float, float, float, float, SpriteEffects, IEffect>(Effects.Cell)(text, color, alphaChannel, rotation, scale, layer, spriteEffect);
        }

        public static Behavior<IEffect> At(this Behavior<IEffect> effect, Behavior<float> x, Behavior<float> y)
        {
            return Behavior.Lift<IEffect, float, float, IEffect>(Effects.Translate)(effect, x, y);
        }
    }
}
