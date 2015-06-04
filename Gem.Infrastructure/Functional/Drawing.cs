using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Infrastructure.Functional
{

    public interface IDrawing
    {
        void Draw(SpriteBatch gr);
    }

    public class Drawing : IDrawing
    {
        public Drawing(Action<SpriteBatch> f)
        {
            this.f = f;
        }
        private readonly Action<SpriteBatch> f;
        public Action<SpriteBatch> DrawFunc { get { return f; } }

        public void Draw(SpriteBatch gr)
        {
            DrawFunc(gr);
        }
    }

    // Static class for creating drawings
    public static class Drawings
    {
        public static IDrawing Compose(this IDrawing img1, IDrawing img2)
        {
            // Create composed drawing
            return new Drawing(g =>
            {
                img1.Draw(g);
                img2.Draw(g);
            }
            );
        }
    }

    public static class Anims
    {
        public static Behavior<IDrawing> Compose(this Behavior<IDrawing> anim1, Behavior<IDrawing> anim2)
        {
            return Behavior.Lift<IDrawing, IDrawing, IDrawing>(Drawings.Compose)(anim1, anim2);
        }
    }
}