using Gem.DrawingSystem.Animations;
using System;

namespace Gem.IDE.Modules.SpriteSheets.Views
{
    public interface ISceneView
    {
        event EventHandler<EventArgs> OnGraphicsDeviceLoaded;

        void Invalidate(AnimationStripSettings settings);

        string Path { get; set; }

        void SetOptions(AnimationViewOptions options);
    }
}
