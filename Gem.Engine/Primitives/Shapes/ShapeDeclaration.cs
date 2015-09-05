using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gem.Engine.Primitives
{
    public class ShapeDeclaration
    {
        private List<Vector2> verticesPosition = new List<Vector2>();
        private readonly Vector2 previous;
        private readonly float captureThreshold = 0.5f;


        [JsonIgnore]
        public Vector2 ViewportOffset { get; }

        public ShapeDeclaration(Vector2 initialPosition)
        {
            previous = initialPosition;
            ViewportOffset = initialPosition;
            verticesPosition.Add(initialPosition - ViewportOffset);
        }

        public bool Capture(Vector2 newLocation)
        {
            if ((newLocation - previous).Length() < captureThreshold)
            {
                return false;
            }
            verticesPosition.Add(newLocation - ViewportOffset);
            return true;
        }

        public List<Vector2> VerticesPosition
        {
            get { return verticesPosition; }
        }

    }
}
