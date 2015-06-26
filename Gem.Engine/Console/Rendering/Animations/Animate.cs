using Gem.Infrastructure.Functional;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Engine.Console.Rendering.Animations
{
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
