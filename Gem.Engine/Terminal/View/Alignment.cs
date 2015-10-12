using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.GTerminal.View
{
    public interface IAlignment
    {
        Rectangle CalculateSafeArea(IView view, Rectangle parentBoundaries, List<Rectangle> siblingBoundaries);
    }

    public class LeftAlignment : IAlignment
    {
        public Rectangle CalculateSafeArea(IView view, Rectangle parentBoundaries, List<Rectangle> siblingBoundaries)
        {
            var area = view.Margin.Bounds;

            var startingPoint = new Point(parentBoundaries.Left, parentBoundaries.Top);

            siblingBoundaries.Where(rect => rect.Contains(startingPoint));
            return Rectangle.Empty;
        }
    }
}
