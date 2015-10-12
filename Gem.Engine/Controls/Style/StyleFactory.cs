using Gem.Engine.Controls.Rendering;
using Gem.Engine.Controls.Structure;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Gem.Infrastructure;

namespace Gem.Engine.Controls.Style
{
    public class Style
    {
        private class SomeElement : IGuiElement
        {
            public string Group
            {
                get; set;
            }
            public string Id { get; set; }
        }
        public static IRenderer Element<TElement>(Predicate<TElement> element)
            where TElement : IGuiElement
        {
            DoStyle<SomeElement>(
                selector =>
                selector.ChildrenOf<SomeElement>(),
                (rule, time) =>
                rule.BackgroundColorGetter = () => Color.White * (float)time);

            return null;
        }

        public class Animation
        {
            private readonly List<Action<double>> animations;
            private readonly List<Action<double>> animationRemovals;

            //public Action<double> CreateFor(TimeSpan span,Action<double> action)
            //{
            //    Action<double> animation = time =>
            //    {
            //        action(time);
            //        if(span.Milliseconds>time)
            //        {
            //            var disposable = Disposable.Create(animations, animation);

            //        }
            //    };
            //    animations.Add(animation);           
              
            //}
        }

        public class TreeSelector
        {
            public bool ChildrenOf<ElementType>() { return true; }
        }
        public static void DoStyle<TElement>(Predicate<TreeSelector> selector, Action<StylingRule,double> ruleSet)
          where TElement : class, IGuiElement
        {
            TElement element = null; //select from tree using tree selector;
            var rule = new StylingRule();

            stylingrules.Add((time) => ruleSet(rule, time));
        }
        private static List<Action<double>> stylingrules = new List<Action<double>>();

        public void Update(double timeDelta)
        {
            stylingrules.ForEach(rule => rule.Invoke(timeDelta));
        }
    }
}
