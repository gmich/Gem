using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Diagnostics.DebugViewFarseer
{
    public class PrimitiveBatch : IDisposable
    {
        private const int DefaultBufferSize = 500;

        // a basic effect, which contains the shaders that we will use to draw our
        // primitives.
        private BasicEffect  basicEffect;

        // the device that we will issue draw calls to.
        private GraphicsDevice  device;

        // hasBegun is flipped to true once Begin is called, and is used to make
        // sure users don't call End before Begin is called.
        private bool  hasBegun;

        private bool  isDisposed;
        private VertexPositionColor[]  lineVertices;
        private int  lineVertsCount;
        private VertexPositionColor[]  triangleVertices;
        private int  triangleVertsCount;

        public PrimitiveBatch(GraphicsDevice graphicsDevice, int bufferSize = DefaultBufferSize)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");

             device = graphicsDevice;

             triangleVertices = new VertexPositionColor[bufferSize - bufferSize % 3];
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

        public void SetProjection(ref Matrix projection)
        {
             basicEffect.Projection = projection;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && ! isDisposed)
            {
                if ( basicEffect != null)
                     basicEffect.Dispose();

                 isDisposed = true;
            }
        }


        /// <summary>
        /// Begin is called to tell the PrimitiveBatch what kind of primitives will be
        /// drawn, and to prepare the graphics card to render those primitives.
        /// </summary>
        /// <param name="projection">The projection.</param>
        /// <param name="view">The view.</param>
        public void Begin(ref Matrix projection, ref Matrix view)
        {
            if ( hasBegun)
                throw new InvalidOperationException("End must be called before Begin can be called again.");

            //tell our basic effect to begin.
             basicEffect.Projection = projection;
             basicEffect.View = view;
             basicEffect.CurrentTechnique.Passes[0].Apply();

            // flip the error checking boolean. It's now ok to call AddVertex, Flush,
            // and End.
             hasBegun = true;
        }

        public bool IsReady()
        {
            return  hasBegun;
        }

        public void AddVertex(Vector2 vertex, Color color, PrimitiveType primitiveType)
        {
            if (! hasBegun)
                throw new InvalidOperationException("Begin must be called before AddVertex can be called.");

            if (primitiveType == PrimitiveType.LineStrip || primitiveType == PrimitiveType.TriangleStrip)
                throw new NotSupportedException("The specified primitiveType is not supported by PrimitiveBatch.");

            if (primitiveType == PrimitiveType.TriangleList)
            {
                if ( triangleVertsCount >=  triangleVertices.Length)
                    FlushTriangles();

                 triangleVertices[ triangleVertsCount].Position = new Vector3(vertex, -0.1f);
                 triangleVertices[ triangleVertsCount].Color = color;
                 triangleVertsCount++;
            }

            if (primitiveType == PrimitiveType.LineList)
            {
                if ( lineVertsCount >=  lineVertices.Length)
                    FlushLines();

                 lineVertices[ lineVertsCount].Position = new Vector3(vertex, 0f);
                 lineVertices[ lineVertsCount].Color = color;
                 lineVertsCount++;
            }
        }

        /// <summary>
        /// End is called once all the primitives have been drawn using AddVertex.
        /// it will call Flush to actually submit the draw call to the graphics card, and
        /// then tell the basic effect to end.
        /// </summary>
        public void End()
        {
            if (! hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before End can be called.");
            }

            // Draw whatever the user wanted us to draw
            FlushTriangles();
            FlushLines();

             hasBegun = false;
        }

        private void FlushTriangles()
        {
            if (! hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before Flush can be called.");
            }
            if ( triangleVertsCount >= 3)
            {
                int primitiveCount =  triangleVertsCount / 3;
                // submit the draw call to the graphics card
                 device.SamplerStates[0] = SamplerState.AnisotropicClamp;
                 device.DrawUserPrimitives(PrimitiveType.TriangleList,  triangleVertices, 0, primitiveCount);
                 triangleVertsCount -= primitiveCount * 3;
            }
        }

        private void FlushLines()
        {
            if (! hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before Flush can be called.");
            }
            if ( lineVertsCount >= 2)
            {
                int primitiveCount =  lineVertsCount / 2;
                // submit the draw call to the graphics card
                 device.SamplerStates[0] = SamplerState.AnisotropicClamp;
                 device.DrawUserPrimitives(PrimitiveType.LineList,  lineVertices, 0, primitiveCount);
                 lineVertsCount -= primitiveCount * 2;
            }
        }
    }
}