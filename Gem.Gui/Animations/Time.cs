using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Animations
{
    public static class Time
    {
        public static Animation<double> Elapsed
        {
            get { return Animation.Create(context => context.TotalMilliseconds); }
        }

        public static Animation<TTime> Constant<TTime>(TTime time)
        {
            return Animation.Create(context => time);
        }

        public static Animation<double> Wave
        {
            get
            {
                return Animation.Create(context => Math.Sin(context.TotalSeconds));
            }
        }
    }
}
