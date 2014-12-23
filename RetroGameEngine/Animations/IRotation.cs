using System;

namespace RetroGameEngine.Animations
{

    public interface IRotation : IAnimation
    {

        void Transform(Func<float> transform);

        float Value { get; }

    }
}
