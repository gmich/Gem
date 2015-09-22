using Gem.CameraSystem;
using Gem.Engine.Containers;
using Gem.Engine.Utilities;
using Microsoft.Xna.Framework;
using System;

namespace Gem.IDE.Infrastructure
{
    public class CameraHandler
    {
        private readonly Range<float> zoomRange;
        private readonly AContainer<TrackedObject> trackedObjects = new AContainer<TrackedObject>();
        private Camera camera;

        public Camera Camera { get { return camera; } }

        public CameraHandler(int positionX, int positionY, int viewportX, int viewportY, float minZoom = 0.2f, float maxZoom = 2.0f)
        {
            camera = new Camera(new Vector2(positionX, positionY), new Vector2(viewportX, viewportY));
            zoomRange = Range.ForFloat(minZoom, maxZoom);
        }

        public void TrackObject(string id, Func<Vector2> position)
        {
            var trackedObject = new TrackedObject(position, () => Camera);
            trackedObjects.Add(id, trackedObject);
        }

        public void Remove(string id)
        {
            trackedObjects.Remove(id);
        }

        public TrackedObject this[string id]
        {
            get
            {
                return trackedObjects[id];
            }
        }

        public void UpdateViewport(int viewportWidth, int viewportHeight)
        {
            camera = new Camera(camera.Position,
                     new Vector2(viewportWidth, viewportHeight));
        }

        public Matrix Matrix
        {
            get { return camera.TransformationMatrix; }
        }

        public float Zoom
        {
            get
            {
                return camera.Zoom.X;
            }
            set
            {
                var zoom = zoomRange.GetNearest(value);
                camera.Zoom = new Vector3(zoom, zoom, 1);
            }
        }

    }
}
