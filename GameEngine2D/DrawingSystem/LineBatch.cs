using System;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine2D.DrawingSystem
{
    public class LineBatch : IDisposable
    {
        private const int DefaultBufferSize = 500;

        // a basic effect, which contains the shaders that we will use to draw our
        // primitives.
        private BasicEffect basicEffect;

        // the device that we will issue draw calls to.
        private GraphicsDevice device;

        // hasBegun is flipped to true once Begin is called, and is used to make
        // sure users don't call End before Begin is called.
        private bool hasBegun;

        private bool isDisposed;
        private VertexPositionColor[] lineVertices;
        private int lineVertsCount;

        public LineBatch(GraphicsDevice graphicsDevice, int bufferSize = DefaultBufferSize)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");

            device = graphicsDevice;

            lineVertices = new VertexPositionColor[bufferSize - bufferSize % 2];

            // set up a new basic effect, and enable vertex colors.
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.VertexColorEnabled = true;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                if (basicEffect != null)
                    basicEffect.Dispose();

                isDisposed = true;
            }
        }

        public void Begin(Matrix projection, Matrix view)
        {
            if (hasBegun)
                throw new InvalidOperationException("End must be called before Begin can be called again.");

            device.SamplerStates[0] = SamplerState.AnisotropicClamp;
            //tell our basic effect to begin.
            basicEffect.Projection = projection;
            basicEffect.View = view;
            basicEffect.CurrentTechnique.Passes[0].Apply();

            // flip the error checking boolean. It's now ok to call DrawLineShape, Flush,
            // and End.
            hasBegun = true;
        }

        public void DrawLineShape(Shape shape)
        {
            DrawLineShape(shape, Color.Black);
        }

        public void DrawLineShape(Shape shape, Color color)
        {
            if (!hasBegun)
                throw new InvalidOperationException("Begin must be called before DrawLineShape can be called.");

            if (shape.ShapeType != ShapeType.Edge && shape.ShapeType != ShapeType.Chain)
                throw new NotSupportedException("The specified shapeType is not supported by LineBatch.");

            if (shape.ShapeType == ShapeType.Edge)
            {
                if (lineVertsCount >= lineVertices.Length)
                    Flush();

                EdgeShape edge = (EdgeShape)shape;
                lineVertices[lineVertsCount].Position = new Vector3(edge.Vertex1, 0f);
                lineVertices[lineVertsCount + 1].Position = new Vector3(edge.Vertex2, 0f);
                lineVertices[lineVertsCount].Color = lineVertices[lineVertsCount + 1].Color = color;
                lineVertsCount += 2;
            }
            else if (shape.ShapeType == ShapeType.Chain)
            {
                ChainShape chain = (ChainShape)shape;
                for (int i = 0; i < chain.Vertices.Count; ++i)
                {
                    if (lineVertsCount >= lineVertices.Length)
                        Flush();

                    lineVertices[lineVertsCount].Position = new Vector3(chain.Vertices[i], 0f);
                    lineVertices[lineVertsCount + 1].Position = new Vector3(chain.Vertices.NextVertex(i), 0f);
                    lineVertices[lineVertsCount].Color = lineVertices[lineVertsCount + 1].Color = color;
                    lineVertsCount += 2;
                }
            }
        }

        public void DrawLine(Vector2 v1, Vector2 v2)
        {
            DrawLine(v1, v2, Color.Black);
        }

        public void DrawLine(Vector2 v1, Vector2 v2, Color color)
        {
            if (!hasBegun)
                throw new InvalidOperationException("Begin must be called before DrawLineShape can be called.");

            if (lineVertsCount >= lineVertices.Length)
                Flush();

            lineVertices[lineVertsCount].Position = new Vector3(v1, 0f);
            lineVertices[lineVertsCount + 1].Position = new Vector3(v2, 0f);
            lineVertices[lineVertsCount].Color = lineVertices[lineVertsCount + 1].Color = color;
            lineVertsCount += 2;
        }

        // End is called once all the primitives have been drawn using AddVertex.
        // it will call Flush to actually submit the draw call to the graphics card, and
        // then tell the basic effect to end.
        public void End()
        {
            if (!hasBegun)
                throw new InvalidOperationException("Begin must be called before End can be called.");

            // Draw whatever the user wanted us to draw
            Flush();

            hasBegun = false;
        }

        private void Flush()
        {
            if (!hasBegun)
                throw new InvalidOperationException("Begin must be called before Flush can be called.");

            if (lineVertsCount >= 2)
            {
                int primitiveCount = lineVertsCount / 2;
                // submit the draw call to the graphics card
                device.DrawUserPrimitives(PrimitiveType.LineList, lineVertices, 0, primitiveCount);
                lineVertsCount -= primitiveCount * 2;
            }
        }
    }
}