using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NullGuard;
using System;

namespace Gem.Engine.Shaders
{
    [NullGuard(ValidationFlags.AllPublicArguments)]
    internal class CastedItem
    {
        private readonly Func<Vector2> positionGetter;
        private readonly Func<Texture2D> textureGetter;

        public Vector2 Position {  get { return positionGetter(); } }
        public Texture2D Texture {  get { return textureGetter(); } }

        public CastedItem(Func<Vector2> positionGetter, Func<Texture2D> textureGetter)
        {
            this.positionGetter = positionGetter;
            this.textureGetter = textureGetter;

        }
    }
}
