using System;

namespace Gem.Animations
{

    public interface IRotation : IAnimation
    {

        void Transform(Func<float> transform);

        float Value { get; }

    }
}
