using System;
using Microsoft.Xna.Framework;

namespace Gem.Network.Shooter.Client.Camera
{
    public interface ICameraScript
    {
        Camera2D Camera
        {
            get;
        }

        void Logic(float timePassed, Vector2 otherLocation,Vector2 otherVelocity,float step);

    }
}
