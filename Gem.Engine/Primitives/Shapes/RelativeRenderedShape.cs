using System;
using Microsoft.Xna.Framework;

namespace Gem.Engine.Primitives
{
    public class RelativeRenderedShape : IShape
    {
        private readonly IShape shape;
        private Func<float, float> XTransformer;
        private readonly Func<float, float> YTransformer;

        public RelativeRenderedShape(IShape shape, Func<float, float> XTransformer, Func<float, float> YTransformer)
        {
            this.shape = shape;
            this.XTransformer = XTransformer;
            this.YTransformer = YTransformer;
        }

        public Color Color
        {
            get
            {
                return shape.Color;
            }
            set
            {
                shape.Color = value;
            }
        }

        public VertexPosition[] VerticesPosition
        {
            get
            {
                return shape.VerticesPosition;
            }
        }

        public float ViewportOffsetX
        {
            get
            {
                return YTransformer(shape.ViewportOffsetX);
            }

            set
            {
                shape.ViewportOffsetX = value;
            }
        }

        public float ViewportOffsetY
        {
            get
            {
                return YTransformer(shape.ViewportOffsetY);
            }
            set
            {
                shape.ViewportOffsetY = value;
            }
        }

        public void Draw()
        {
            shape.Draw();
        }
    }
}
