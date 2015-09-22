using Gem.CameraSystem;
using Microsoft.Xna.Framework;
using System;

namespace Gem.IDE.Infrastructure
{
    public class TrackedObject
    {
        private readonly Func<Vector2> position;
        private readonly Func<Camera> camera;

        public TrackedObject(Func<Vector2> position, Func<Camera> camera)
        {
            this.position = position;
            this.camera = camera;
        }

        public Vector2 RenderPosition
        {
            get
            {
                return camera()
                    // .TranslateWorldToScreen(camera()
                       .Position - position();
            }
        }

    }
}
