using Microsoft.Xna.Framework;
using NullGuard;
using System.Collections.Generic;
using System;
using Gem.Engine.GTerminal.View.Style;

namespace Gem.Engine.GTerminal.View
{
    public interface IView
    {
        IRenderStyle Style { get; set; }
        IEnumerable<IView> Children { get; }
        Box Padding { get; }
        Box Border { get; }
        Box Margin { get; }
        Point Position { get; }
        IAlignment Alignment { get; }
        void Update(double timeDelta);
        void Render(Rectangle parentBoundaries, List<Rectangle> siblingBoundaries, RenderContext context);
        void Render(Rectangle parentBoundaries, RenderContext context);
    }
}
