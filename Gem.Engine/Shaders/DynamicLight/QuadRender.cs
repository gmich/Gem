using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NullGuard;

namespace Gem.Engine.Shaders
{
    [NullGuard(ValidationFlags.AllPublicArguments)]
    public partial class QuadRender
    {
        private readonly VertexPositionTexture[] verts;
        private readonly short[] ib;
        private readonly GraphicsDevice device;

        public QuadRender(GraphicsDevice device)
        {
            this.device = device;
            verts = new VertexPositionTexture[]
            {
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(1,1)),
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(0,1)),
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(0,0)),
                new VertexPositionTexture(
                    new Vector3(0,0,0),
                    new Vector2(1,0))
            };

            ib = new short[] { 0, 1, 2, 2, 3, 0 };
        }


        public void Render(Vector2 v1, Vector2 v2)
        {
            verts[0].Position.X = v2.X;
            verts[0].Position.Y = v1.Y;

            verts[1].Position.X = v1.X;
            verts[1].Position.Y = v1.Y;

            verts[2].Position.X = v1.X;
            verts[2].Position.Y = v2.Y;

            verts[3].Position.X = v2.X;
            verts[3].Position.Y = v2.Y;

            device.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, verts, 0, 4, ib, 0, 2);
        }

    }
}
