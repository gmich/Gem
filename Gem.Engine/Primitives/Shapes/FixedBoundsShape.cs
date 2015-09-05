using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Engine.Primitives
{
    public class FixedBoundsShape : IShape
    {
        private readonly GraphicsDevice device;
        private readonly BasicEffect effect;
        private VertexPositionColor[] vertices;

        public FixedBoundsShape(IEnumerable<Vector2> shapeVertices, float x, float y, Color color, GraphicsDevice device)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            this.device = device;

            var positions = shapeVertices.ToList();

            VerticesPosition = new VertexPosition[positions.Count];
            for (int i = 0; i < positions.Count; i++)
            {
                VerticesPosition[i] = new VertexPosition((int)positions[i].X, (int)positions[i].Y);
            }

            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;
            effect.Projection = Matrix.CreateOrthographicOffCenter
                (0, device.Viewport.Width,
                 device.Viewport.Height, 0,
                 0, 1);

            InitializeVertices();
        }


        #region Properties


        public VertexPosition[] VerticesPosition { get; }

        private float x;
        public float ViewportOffsetX
        {
            get { return x; }
            set { x = value; InitializeVertices(); }
        }
        private float y;
        public float ViewportOffsetY
        {
            get { return y; }
            set { y = value; InitializeVertices(); }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; InitializeVertices(); }
        }


        #endregion

        public void Draw()
        {
            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserPrimitives
                (PrimitiveType.LineStrip, vertices, 0, vertices.Length - 1);
        }


        private void InitializeVertices()
        {
            vertices = new VertexPositionColor[VerticesPosition.Length];
            for (int i = 0; i < VerticesPosition.Length; i++)
            {
                vertices[i].Position = new Vector3(VerticesPosition[i].X + ViewportOffsetX, VerticesPosition[i].Y + ViewportOffsetY, 0);
                vertices[i].Color = Color;
            }
        }

    }
}

