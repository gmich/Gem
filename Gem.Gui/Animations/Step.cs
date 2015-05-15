using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Animations
{
    public sealed class Step
    {
        private readonly Vector2 to;
        private Vector2 from;

        private Step(Vector2 from, Vector2 to)
        {
            this.from = from;
            this.to = to;
        }

        /// <example>
        /// Movement.Create(from: Vector2.Zero,to: Vector2.One,withStep: step=>step.Smooth);
        /// </example>
        public static Animation<Vector2> Create(Vector2 from, Vector2 to, Func<Step, Animation<Vector2>> withStep)
        {
            return withStep(new Step(from, to));
        }

        public Animation<Vector2> Smooth
        {
            get
            {
                return Animation.Create(context => (from - to) * (float)context.TotalSeconds);
            }
        }

    }
}
