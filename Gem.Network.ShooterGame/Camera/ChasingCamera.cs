using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Network.Shooter.Client.Camera
{
    public class ChasingCamera : ICameraScript
    {

        #region Declarations

        private Camera camera;
        private float distance;
        private Vector2 Offset;
        private const float horizontalOffset = 30;
        private const float verticalOffset = 60;

        #endregion

        #region Constructor

        public ChasingCamera(Vector2 worldLocation,Vector2 viewportSize)
        {
            Offset = Vector2.Zero;
            camera = new Camera(worldLocation, viewportSize);
        }

        #endregion

        #region Properties

        public Camera Camera
        {
            get
            {
                return camera;
            }
        }

        public Vector2 offset
        {
            get
            {
                return Offset;
            }
            set
            {
                Offset.X = MathHelper.Clamp(value.X, -horizontalOffset, horizontalOffset);
                Offset.Y = MathHelper.Clamp(value.Y, -verticalOffset, verticalOffset);
            }
        }
        public Vector2 WorldLocation
        {
            get;
            set;
        }

        #endregion

        #region Helper Methods

        #region Scrolling

        public void RepositionCamera(float timePassed)
        {
            int screenLocX = (int)camera.WorldToScreen(WorldLocation).X + (int)offset.X;
            int screenLocY = (int)camera.WorldToScreen(WorldLocation).Y + (int)offset.Y;
            Vector2 angle = WorldLocation+offset - camera.WorldCenter;

            angle.Normalize();

            if (screenLocY > camera.ViewPortHeight / 2)
            {
                camera.Move(angle * distance * timePassed);
            }
            if (screenLocY < camera.ViewPortHeight / 2)
            {
                camera.Move(angle * distance * timePassed);
            }

            if (screenLocX > camera.ViewPortWidth / 2)
            {
                camera.Move(angle * distance * timePassed);
            }

            if (screenLocX < camera.ViewPortWidth / 2)
            {
                camera.Move(angle * distance * timePassed);
            }
        }

        private void CalculateCameraOffset(Vector2 otherLocation)
        {

            if (otherLocation.X > 0)
                offset += new Vector2(horizontalOffset, 0);
            if (otherLocation.X < 0)
                offset -= new Vector2(horizontalOffset, 0);
            if (otherLocation.Y > 0)
                offset -= new Vector2(0, verticalOffset);
            if (otherLocation.Y < 0)
                offset -= new Vector2(0, verticalOffset);

        }

        public void Logic(float timePassed, Vector2 otherLocation,Vector2 otherVelocity,float step)
        {
            CalculateCameraOffset(otherVelocity);
            WorldLocation = otherLocation;
            
            distance = Vector2.Distance(otherLocation+offset, camera.WorldCenter);
            distance *= 4;

            if (distance < 0.001f)
                distance = 0.0f;

            RepositionCamera(timePassed);
        }

        #endregion
        #endregion
        
    }
}
