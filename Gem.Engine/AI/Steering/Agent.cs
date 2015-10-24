using Microsoft.Xna.Framework;
using System;

namespace Gem.Engine.AI.Steering
{
    public class Agent
    {
        public Agent(Func<Vector2> velocity, Func<Vector2> position)
        {
            VelocityProvider = velocity;
            PositionProvider = position;
        }

        internal Func<Vector2> PositionProvider;
        internal Func<Vector2> VelocityProvider { get; set; }

        public Vector2 Velocity { get { return VelocityProvider(); } }
        public Vector2 Position { get { return PositionProvider(); } }
        public float AlignmentWeight { get; set; } = 1.0f;
        public float CohesionWeight { get; set; } = 1.0f;
        public float SeparationWeight { get; set; } = 1.0f;
    }
}
