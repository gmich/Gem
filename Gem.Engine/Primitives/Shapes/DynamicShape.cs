using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Engine.Primitives
{
    public class DynamicShape : IShape
    {
        private readonly GraphicsDevice device;
        private readonly BasicEffect effect;
        private List<VertexPositionColor> vertices = new List<VertexPositionColor>();

        public DynamicShape(Color color, GraphicsDevice device,int viewportWidth, int viewportHeight)
        {
            Color = color;
            this.device = device;
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;
            effect.Projection = Matrix.CreateOrthographicOffCenter
                (0, viewportWidth,
                 viewportHeight, 0,
                 0, 1);
        }


        #region Properties

        public float ViewportOffsetX
        {
            get; set;
        }
        public float ViewportOffsetY
        {
            get; set;
        }

        public Color Color
        {
            get; set;
        }

        public VertexPosition[] VerticesPosition =>
            vertices
            .Select(vertex => new VertexPosition((int)vertex.Position.X, (int)vertex.Position.Y))
            .ToArray();

        #endregion

        public void Draw()
        {
            if (vertices.Count == 0) return;
            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserPrimitives
                (PrimitiveType.LineStrip, vertices.ToArray(), 0, vertices.Count - 1);
        }

        public void AddVertex(Vector2 vertexPosition)
        {
            var vertex = new VertexPositionColor(new Vector3(vertexPosition.X + ViewportOffsetX, vertexPosition.Y + ViewportOffsetY, 0), Color);
            vertices.Add(vertex);
        }

        public void Reset()
        {
            vertices.Clear();
        }
        
    }
}
