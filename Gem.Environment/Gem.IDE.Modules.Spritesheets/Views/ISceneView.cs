using Gem.DrawingSystem.Animations;
using System;

namespace Gem.IDE.Modules.SpriteSheets.Views
{
    public interface ISceneView : IDisposable
    {
        event EventHandler<EventArgs> OnGraphicsDeviceLoaded;

        void Invalidate(AnimationStripSettings settings);

        void SetOptions(AnimationViewOptions options);

        Tuple<int,int,byte[]> LoadTexture(string path);

        void SetColorData(byte[] data, int width, int height);
    }
}
