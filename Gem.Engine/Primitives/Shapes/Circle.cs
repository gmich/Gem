using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace Gem.Engine.Primitives
{
    public class Circle : IShape
    {
        private GraphicsDevice device;
        private VertexPositionColor[] vertices;
        private BasicEffect effect;
        
        public Circle(float x, float y, int radius,
            Color color, GraphicsDevice device)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.color = color;
            this.device = device;

            Initialize();
        }

        public static Texture2D CreateTexture(GraphicsDevice device,int radius, Color inner, Color outer)
        {
            int outerRadius = radius * 2 + 2;
            Texture2D texture = new Texture2D(device, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            for (int i = 0; i < data.Length; i++)
                data[i] = inner;

            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = outer;
            }

            texture.SetData(data);
            return texture;
        }

        public void Draw()
        {
            effect.CurrentTechnique.Passes[0].Apply();
            device.DrawUserPrimitives
                (PrimitiveType.LineStrip, vertices, 0, vertices.Length - 1);
        }

        private void Initialize()
        {
            InitializeBasicEffect();
            InitializeVertices();
        }

        private void InitializeBasicEffect()
        {
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;
            effect.Projection = Matrix.CreateOrthographicOffCenter
                (0, device.Viewport.Width,
                 device.Viewport.Height, 0,
                 0, 1);
        }

        private void InitializeVertices()
        {
            vertices = new VertexPositionColor[CalculatePointCount()];
            var pointTheta = ((float)Math.PI * 2) / (vertices.Length - 1);
            for (int i = 0; i < vertices.Length; i++)
            {
                var theta = pointTheta * i;
                var x = ViewportOffsetX + ((float)Math.Sin(theta) * Radius);
                var y = ViewportOffsetY + ((float)Math.Cos(theta) * Radius);
                vertices[i].Position = new Vector3(x, y, 0);
                vertices[i].Color = Color;
            }
            vertices[vertices.Length - 1] = vertices[0];
        }

        public int Points
        {
            get { return CalculatePointCount(); }
        }


        private int CalculatePointCount()
        {
            return (int)Math.Ceiling(Radius * Math.PI);
        }

        #region Properties
        
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
        private float radius;
        public float Radius
        {
            get { return radius; }
            set { radius = (value < 1) ? 1 : value; InitializeVertices(); }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; InitializeVertices(); }
        }

        public VertexPosition[] VerticesPosition => 
            vertices
            .Select(vertex => new VertexPosition((int)vertex.Position.X, (int)vertex.Position.Y))
            .ToArray();

        #endregion

    }
}

