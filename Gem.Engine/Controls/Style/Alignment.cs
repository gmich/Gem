using Gem.Engine.Controls.Structure;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Engine.Controls.Style
{
    public interface IAlignment
    {
        Rectangle CalculateSafeArea(IGuiElement view, Rectangle parentBoundaries, List<Rectangle> siblingBoundaries);
    }

    public class LeftAlignment : IAlignment
    {
        public Rectangle CalculateSafeArea(IGuiElement view, Rectangle parentBoundaries, List<Rectangle> siblingBoundaries)
        {
           // var area = view.Margin.Bounds;

            var startingPoint = new Point(parentBoundaries.Left, parentBoundaries.Top);

            siblingBoundaries.Where(rect => rect.Contains(startingPoint));
            return Rectangle.Empty;
        }
    }

    public sealed class Alignment
    {
        public static IAlignment Auto
        {
            get { return null; }
        }
    }
}
