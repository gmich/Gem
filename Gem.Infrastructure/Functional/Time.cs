using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Infrastructure.Functional
{
    public static class Time
    {
        // Behavior that represents the current time
        public static Behavior<float> Current
        {
            get { return Behavior.Create(ctx => ctx.Time); }
        }

        // A value oscillating between 1 and -1
        public static Behavior<float> Wiggle
        {
            get
            {
                return Behavior.Create(ctx => (float)Math.Sin(ctx.Time * Math.PI));
            }
        }

        // Create constant behavior from a value
        public static Behavior<T> Forever<T>(T v)
        {
            return Behavior.Create(ctx => v);
        }

        // Extension method for floats only
        public static Behavior<float> Forever(this float v)
        {
            return Behavior.Create(ctx => v);
        }

        public static Behavior<T> Faster<T>(this Behavior<T> v, float speed)
        {
            return Behavior.Create(ctx => v.BehaviorFunc(new BehaviorContext(ctx.Time * speed)));
        }

        public static Behavior<T> Wait<T>(this Behavior<T> v, float delay)
        {
            return Behavior.Create(ctx => v.BehaviorFunc(new BehaviorContext(ctx.Time + delay)));
        }
    }

}
