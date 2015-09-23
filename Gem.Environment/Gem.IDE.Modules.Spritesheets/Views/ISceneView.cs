using Gem.DrawingSystem.Animations;
using System;

namespace Gem.IDE.Modules.SpriteSheets.Views
{
    public interface ISceneView : IDisposable
    {
        event EventHandler<EventArgs> OnGraphicsDeviceLoaded;

        event EventHandler<double> onScaleChange;

        void Invalidate(AnimationStripSettings settings, Action<AnimationStripSettings> saveSettingsCallback);

        void SetOptions(AnimationViewOptions options);

        Tuple<int,int,byte[]> LoadTexture(string path);

        void SetColorData(byte[] data, int width, int height);

        double Scale { get; set; }

        Microsoft.Xna.Framework.Color BackgroundColor { get; set; }
    }
}
