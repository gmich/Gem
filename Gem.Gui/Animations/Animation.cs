using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Animations
{
    public class Animation<T>
    {
        private readonly Func<AnimationContext, T> animationBehavior;

        public Animation(Func<AnimationContext, T> animationBehavior)
        {
            this.animationBehavior = animationBehavior;
        }

        public Func<AnimationContext, T> Animate
        {
            get { return animationBehavior; }
        }
    }

    public static class Animation
    {
        public static Animation<T> Create<T>(Func<AnimationContext, T> f)
        {
            return new Animation<T>(f);
        }

        public static Animation<Second> Select<First, Second>(this Animation<First> animation, Func<First, Second> f)
        {
            return Animation.Create(context => f(animation.Animate(context)));
        }

        public static Func<Animation<First>,Animation<Second>> Lift<First, Second>(this Func<First,Second> f)
        {
            return animation => Create(context => f(animation.Animate(context)));
        }

        public static Func<Animation<First>, Animation<Second>, Animation<Third>> Lift<First, Second,Third>(this Func<First, Second, Third> f)
        {
            return (animation1, animation2) => Create(context => f(animation1.Animate(context),animation2.Animate(context)));
        }
    }
}
