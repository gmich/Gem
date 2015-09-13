using Gem.DrawingSystem.Animations;

namespace Gem.IDE.Modules.SpriteSheets.Views
{
    public interface ISceneView
    {
        void Invalidate(AnimationStripSettings settings);
    }
}
