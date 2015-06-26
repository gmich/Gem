using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gem.Engine.Utilities;

namespace Gem.CameraSystem
{

    public sealed class Camera
    {
        #region Fields

        private readonly Vector2 viewportSize;
        private readonly Vector2 virtualSize;
        private readonly Range<float> zoomRange;

        private Vector2 position;
        private Vector3 zoom;
        private float rotation;

        #endregion

        #region Constructor

        public Camera(Vector2 initialPos, Vector2 viewportSize, Vector2 virtualSize, float minZoom = 0.2f, float maxZoom = 2.0f)
        {
            this.Position = initialPos;
            this.virtualSize = virtualSize;
            this.viewportSize = viewportSize;
            this.zoomRange = Range.ForFloat(minZoom, maxZoom);
            Update();
        }

        #endregion

        #region Properties

        private float ViewPortWidth
        {
            get { return viewportSize.X; }
        }

        private float ViewPortHeight
        {
            get { return viewportSize.Y; }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                float x = MathHelper.Clamp(value.X, 0, virtualSize.X - viewportSize.X);
                float y = MathHelper.Clamp(value.Y, 0, virtualSize.Y - viewportSize.Y);
                position = new Vector2(x, y);
                Update();
            }
        }

        public Rectangle ViewPort
        {
            get;
            private set;
        }

        public Rectangle VisibleArea
        {
            get;
            private set;
        }

        #endregion

        #region Translation Properties

        public Vector3 Zoom
        {
            get { return zoom; }
            set
            {
                zoom.X = zoomRange.GetNearest(value.X);
                zoom.Y = zoomRange.GetNearest(value.Y);
                Update();
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; Update(); }
        }

        public Matrix TransformationMatrix
        {
            get;
            private set;
        }

        #endregion

        #region Public Methods

        public bool IsVisible(Vector2 location)
        {
            return (location.X > Position.X && location.X < Position.X + ViewPortWidth
                 && location.Y > Position.Y && location.Y < Position.Y + ViewPortHeight);
        }

        public bool IsVisible(Rectangle bounds)
        {
            return (ViewPort.Intersects(bounds));
        }

        public Vector2 AdjustInWorldBounds(Vector2 location, float width, float height)
        {
            location.X = MathHelper.Clamp(location.X, position.X, position.X + ViewPortWidth - width);
            location.Y = MathHelper.Clamp(location.Y, position.Y, position.Y + ViewPortHeight - height);
            return location;
        }

        public Vector2 AdjustInWorldBounds(Vector2 location, float width, float height, Vector2 origin)
        {
            location.X = MathHelper.Clamp(location.X, position.X + origin.X, position.X + ViewPortWidth - width + origin.X);
            location.Y = MathHelper.Clamp(location.Y, position.Y + origin.Y, position.Y + ViewPortHeight - height + origin.Y);
            return location;
        }

        public Vector2 WorldToScreen(Vector2 worldLocation)
        {
            return worldLocation - position;
        }

        public Rectangle WorldToScreen(Rectangle worldRectangle)
        {
            return new Rectangle(worldRectangle.Left - (int)position.X, worldRectangle.Top - (int)position.Y, worldRectangle.Width, worldRectangle.Height);
        }

        public Vector2 ScreenToWorld(Vector2 screenLocation)
        {
            return screenLocation + position;
        }

        public Rectangle ScreenToWorld(Rectangle screenRectangle)
        {
            return new Rectangle(screenRectangle.Left + (int)position.X, screenRectangle.Top + (int)position.Y, screenRectangle.Width, screenRectangle.Height);
        }

        #endregion

        #region Translations

        private void CalculateVisibleArea()
        {
            var inverseViewMatrix = Matrix.Invert(TransformationMatrix);
            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(viewportSize.X, 0), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, viewportSize.Y), inverseViewMatrix);
            var br = Vector2.Transform(viewportSize, inverseViewMatrix);
            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
            VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        private void CalculateTransformationMatrix()
        {
            TransformationMatrix = Matrix.CreateTranslation(new Vector3(ViewPortWidth / 2, -ViewPortHeight / 2, 0)) *
                                                 Matrix.CreateRotationZ(Rotation) *
                                                 Matrix.CreateScale(new Vector3(Zoom.X, Zoom.Y, Zoom.Z)) *
                                                 Matrix.CreateTranslation(new Vector3(ViewPortWidth / 2, ViewPortHeight / 2, 0));
        }

        public Vector2 TranslateScreenToWorld(Vector2 location)
        {
            return Vector2.Transform(location, Matrix.Invert(TransformationMatrix));
        }

        public Vector2 TranslateWorldToScreen(Vector2 location)
        {
            return Vector2.Transform(location, TransformationMatrix);
        }

        #endregion

        #region Update

        private void Update()
        {
            ViewPort = new Rectangle((int)Position.X, (int)Position.Y, (int)viewportSize.X, (int)viewportSize.Y);
            CalculateTransformationMatrix();
        }

        #endregion
    }
}
