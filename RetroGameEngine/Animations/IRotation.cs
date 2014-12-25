using System;

namespace GameEngine2D.Animations
{

    public interface IRotation : IAnimation
    {

        void Transform(Func<float> transform);

        float Value { get; }

    }
}
