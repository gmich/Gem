using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Infrastructure.Functional
{
    // Immutable animation state
    internal struct BehaviorContext
    {
        public BehaviorContext(float time)
        {
            this.time = time;
        }
        private readonly float time;

        public float Time { get { return time; } }
    }

    public class Behavior<T>
    {
        internal Behavior(Func<BehaviorContext, T> f)
        {
            this.f = f;
        }
        private Func<BehaviorContext, T> f;

        // Function that calculates the value
        internal Func<BehaviorContext, T> BehaviorFunc { get { return f; } }

        internal Behavior<float> ToSingle()
        {
            return Behavior.Create(ctx => Convert.ToSingle(this.BehaviorFunc(ctx)));
        }
        public static Behavior<float> operator *(Behavior<T> a, Behavior<float> b)
        {
            return Behavior.Lift((float x, float y) => x * y)(a.ToSingle(), b);
        }
        public static Behavior<float> operator +(Behavior<T> a, Behavior<float> b)
        {
            return Behavior.Lift((float x, float y) => x + y)(a.ToSingle(), b);
        }
    }

    internal static class Behavior
    {
        internal static Behavior<T> Create<T>(Func<BehaviorContext, T> f)
        {
            return new Behavior<T>(f);
        }

        public static Behavior<R> Select<T, R>(this Behavior<T> behavior, Func<T, R> f)
        {
            return Create(ctx => f(behavior.BehaviorFunc(ctx)));
        }

        public static Func<Behavior<T>, Behavior<R>> Lift<T, R>(Func<T, R> f)
        {
            return (b) => Create(ctx => f(b.BehaviorFunc(ctx)));
        }

        public static Func<Behavior<T1>, Behavior<T2>, Behavior<R>> Lift<T1, T2, R>(Func<T1, T2, R> f)
        {
            return (b1, b2) => Create(ctx =>
              f(b1.BehaviorFunc(ctx), b2.BehaviorFunc(ctx)));
        }

        public static Func<Behavior<T1>, Behavior<T2>, Behavior<T3>, Behavior<R>> Lift<T1, T2, T3, R>(Func<T1, T2, T3, R> f)
        {
            return (b1, b2, b3) =>
                Create(ctx => f(b1.BehaviorFunc(ctx), b2.BehaviorFunc(ctx), b3.BehaviorFunc(ctx)));
        }
    }
}
