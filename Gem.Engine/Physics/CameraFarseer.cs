using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using Gem.Engine.GameLoop;

namespace Gem.Engine.Physics
{
    /// <summary>
    /// A camera for tracking Farseer physics objects
    /// </summary>
    public class CameraFarseer
    {

        #region Private Fields

        private static GraphicsDevice graphics;

        private const float MinZoom = 0.02f;
        private const float MaxZoom = 20f;

        private Vector2 currentPosition;
        private float currentRotation;
        private float currentZoom;
        private Vector2 maxPosition;
        private float maxRotation;
        private Vector2 minPosition;
        private float minRotation;
        private bool positionTracking;
        private bool rotationTracking;
        private Vector2 targetPosition;
        private float targetRotation;
        private Body trackingBody;
        private Vector2 translateCenter;

        public Matrix SimProjection;
        public Matrix SimView;
        public Matrix View;

        #endregion


        #region Constructor

        public CameraFarseer(GraphicsDevice graphics)
        {
            CameraFarseer.graphics = graphics;
            SimProjection = Matrix.CreateOrthographicOffCenter(0f, ConvertUnits.ToSimUnits(graphics.Viewport.Width), ConvertUnits.ToSimUnits(graphics.Viewport.Height), 0f, 0f, 1f);
            SimView = Matrix.Identity;
            View = Matrix.Identity;

            translateCenter = new Vector2(ConvertUnits.ToSimUnits(graphics.Viewport.Width / 2f), ConvertUnits.ToSimUnits(graphics.Viewport.Height / 2f));

            Reset();
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// The current position of the camera.
        /// </summary>
        public Vector2 Position
        {
            get { return ConvertUnits.ToDisplayUnits(currentPosition); }
            set
            {
                targetPosition = ConvertUnits.ToSimUnits(value);
                if (minPosition != maxPosition)
                {
                    Vector2.Clamp(ref targetPosition, ref minPosition, ref maxPosition, out targetPosition);
                }
            }
        }

        /// <summary>
        /// The furthest up, and the furthest left the camera can go.
        /// if this value equals maxPosition, then no clamping will be 
        /// applied (unless you override that function).
        /// </summary>
        public Vector2 MinPosition
        {
            get { return ConvertUnits.ToDisplayUnits(minPosition); }
            set { minPosition = ConvertUnits.ToSimUnits(value); }
        }

        /// <summary>
        /// the furthest down, and the furthest right the camera will go.
        /// if this value equals minPosition, then no clamping will be 
        /// applied (unless you override that function).
        /// </summary>
        public Vector2 MaxPosition
        {
            get { return ConvertUnits.ToDisplayUnits(maxPosition); }
            set { maxPosition = ConvertUnits.ToSimUnits(value); }
        }

        /// <summary>
        /// The current rotation of the camera in radians.
        /// </summary>
        public float Rotation
        {
            get { return currentRotation; }
            set
            {
                targetRotation = value % MathHelper.TwoPi;
                if (minRotation != maxRotation)
                {
                    targetRotation = MathHelper.Clamp(targetRotation, minRotation, maxRotation);
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum rotation in radians.
        /// </summary>
        /// <value>The min rotation.</value>
        public float MinRotation
        {
            get { return minRotation; }
            set { minRotation = MathHelper.Clamp(value, -MathHelper.Pi, 0f); }
        }

        /// <summary>
        /// Gets or sets the maximum rotation in radians.
        /// </summary>
        /// <value>The max rotation.</value>
        public float MaxRotation
        {
            get { return maxRotation; }
            set { maxRotation = MathHelper.Clamp(value, 0f, MathHelper.Pi); }
        }

        /// <summary>
        /// The current rotation of the camera in radians.
        /// </summary>
        public float Zoom
        {
            get { return currentZoom; }
            set
            {
                currentZoom = value;
                currentZoom = MathHelper.Clamp(currentZoom, MinZoom, MaxZoom);
            }
        }

        /// <summary>
        /// the body that this camera is currently tracking. 
        /// Null if not tracking any.
        /// </summary>
        public Body TrackingBody
        {
            get { return trackingBody; }
            set
            {
                trackingBody = value;
                if (trackingBody != null)
                {
                    positionTracking = true;
                }
            }
        }

        public bool EnablePositionTracking
        {
            get { return positionTracking; }
            set
            {
                if (value && trackingBody != null)
                {
                    positionTracking = true;
                }
                else
                {
                    positionTracking = false;
                }
            }
        }

        public bool EnableRotationTracking
        {
            get { return rotationTracking; }
            set
            {
                if (value && trackingBody != null)
                {
                    rotationTracking = true;
                }
                else
                {
                    rotationTracking = false;
                }
            }
        }

        public bool EnableTracking
        {
            set
            {
                EnablePositionTracking = value;
                EnableRotationTracking = value;
            }
        }


        #endregion 


        #region Camera Transformation

        public void MoveCamera(Vector2 amount)
        {
            currentPosition += amount;
            if (minPosition != maxPosition)
            {
                Vector2.Clamp(ref currentPosition, ref minPosition, ref maxPosition, out currentPosition);
            }
            targetPosition = currentPosition;
            positionTracking = false;
            rotationTracking = false;
        }

        public void RotateCamera(float amount)
        {
            currentRotation += amount;
            if (minRotation != maxRotation)
            {
                currentRotation = MathHelper.Clamp(currentRotation, minRotation, maxRotation);
            }
            targetRotation = currentRotation;
            positionTracking = false;
            rotationTracking = false;
        }

        /// <summary>
        /// Resets the camera to default values.
        /// </summary>
        public void Reset()
        {
            currentPosition = Vector2.Zero;
            targetPosition = Vector2.Zero;
            minPosition = Vector2.Zero;
            maxPosition = Vector2.Zero;

            currentRotation = 0f;
            targetRotation = 0f;
            minRotation = -MathHelper.Pi;
            maxRotation = MathHelper.Pi;

            positionTracking = false;
            rotationTracking = false;

            currentZoom = 1f;

            SetView();
        }

        public void Jump2Target()
        {
            currentPosition = targetPosition;
            currentRotation = targetRotation;

            SetView();
        }

        private void SetView()
        {
            Matrix matRotation = Matrix.CreateRotationZ(currentRotation);
            Matrix matZoom = Matrix.CreateScale(currentZoom);
            Vector3 _translateCenter = new Vector3(translateCenter, 0f);
            Vector3 translateBody = new Vector3(-currentPosition, 0f);

            SimView = Matrix.CreateTranslation(translateBody) * matRotation * matZoom * Matrix.CreateTranslation(_translateCenter);

            _translateCenter = ConvertUnits.ToDisplayUnits(_translateCenter);
            translateBody = ConvertUnits.ToDisplayUnits(translateBody);

            View = Matrix.CreateTranslation(translateBody) * matRotation * matZoom * Matrix.CreateTranslation(_translateCenter);
        }


        #endregion


        #region Update

        /// <summary>
        /// Moves the camera forward one timestep.
        /// </summary>
        public void Update(ITimeline time)
        {
            if (trackingBody != null)
            {
                if (positionTracking)
                {
                    targetPosition = trackingBody.Position;
                    if (minPosition != maxPosition)
                    {
                        Vector2.Clamp(ref targetPosition, ref minPosition, ref maxPosition, out targetPosition);
                    }
                }
                if (rotationTracking)
                {
                    targetRotation = -trackingBody.Rotation % MathHelper.TwoPi;
                    if (minRotation != maxRotation)
                    {
                        targetRotation = MathHelper.Clamp(targetRotation, minRotation, maxRotation);
                    }
                }
            }
            Vector2 delta = targetPosition - currentPosition;
            float distance = delta.Length();
            if (distance > 0f)
            {
                delta /= distance;
            }
            float inertia;
            if (distance < 10f)
            {
                inertia = (float)Math.Pow(distance / 10.0, 2.0);
            }
            else
            {
                inertia = 1f;
            }

            float rotDelta = targetRotation - currentRotation;

            float rotInertia;
            if (Math.Abs(rotDelta) < 5f)
            {
                rotInertia = (float)Math.Pow(rotDelta / 5.0, 2.0);
            }
            else
            {
                rotInertia = 1f;
            }
            if (Math.Abs(rotDelta) > 0f)
            {
                rotDelta /= Math.Abs(rotDelta);
            }

            currentPosition += 100f * delta * inertia * (float)time.DeltaTime.TotalSeconds;
            currentRotation += 80f * rotDelta * rotInertia * (float)time.DeltaTime.TotalSeconds;

            SetView();
        }

        #endregion


        #region Convertions

        public Vector2 ConvertScreenToWorld(Vector2 location)
        {
            Vector3 t = new Vector3(location, 0);
            t = graphics.Viewport.Unproject(t, SimProjection, SimView, Matrix.Identity);
            return new Vector2(t.X, t.Y);
        }

        public Vector2 ConvertWorldToScreen(Vector2 location)
        {
            Vector3 t = new Vector3(location, 0);
            t = graphics.Viewport.Project(t, SimProjection, SimView, Matrix.Identity);
            return new Vector2(t.X, t.Y);
        }

        #endregion

    }
}