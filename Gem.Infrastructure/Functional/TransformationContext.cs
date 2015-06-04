using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Infrastructure.Functional
{
//    class Camera
//    {
//        private Matrix TransformMatrix
//    { 
//        get {
//            return 
//                Matrix.CreateTranslation(new Vector3(-Location.X, -Location.Y, 0)) *
//                Matrix.CreateRotationZ(Rotation) *
//                Matrix.CreateScale(Zoom) *
//                Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
//        }
//    };

//        public Rectangle VisibleArea {
//    get {
//        var inverseViewMatrix = Matrix.Invert(View);
//        var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
//        var tr = Vector2.Transform(new Vector2(_screenSize.X, 0), inverseViewMatrix);
//        var bl = Vector2.Transform(new Vector2(0, _screenSize.Y), inverseViewMatrix);
//        var br = Vector2.Transform(_screenSize, inverseViewMatrix);
//        var min = new Vector2(
//            MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))), 
//            MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
//        var max = new Vector2(
//            MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))), 
//            MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
//        return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
//    }
//}
//    }
//    internal struct TransformationContext
//    {
//        public TransformationContext(Camera time)
//        {
//            this.time = time;
//        }
//        private readonly float time;

//        public float Time { get { return time; } }
    }

