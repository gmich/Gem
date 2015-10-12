using Microsoft.Xna.Framework;

namespace Gem.Engine.GTerminal.View.Style
{
    public interface IRenderStyle
    {
        void Update(double timeDelta);
        void Render(IView view,Rectangle parentBoundaries, RenderContext context);
    }
}
