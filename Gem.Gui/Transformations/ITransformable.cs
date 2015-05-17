using Gem.Gui.Rendering;
using Gem.Gui.Transformations;
using System;

namespace Gem.Gui.Transformations
{
    public interface ITransformable : IRenderable
    {
        IDisposable AddTransformation(ITransformation transformation);

        void Update(double deltaTime);
    }
}
