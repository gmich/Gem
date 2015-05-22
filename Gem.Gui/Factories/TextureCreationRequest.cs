using Gem.Gui.Styles;
using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Factories
{
    internal class TextureCreationRequest : IEquatable<TextureCreationRequest>
    {
        private readonly int width;
        private readonly int height;
        private readonly IColorPattern pattern;

        public TextureCreationRequest(int width, int height, IColorPattern pattern)
        {
            this.width = width;
            this.height = height;
            this.pattern = pattern;
        }

        public int Width { get { return width; } }

        public int Height { get { return height; } }

        public IColorPattern Pattern { get { return pattern; } }

        #region IEquatable Members

        public override bool Equals(object right)
        {
            if (Object.ReferenceEquals(right, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, right))
            {
                return true;
            }
            if (this.GetType() != right.GetType())
            {
                return false;
            }
            return this.Equals(right as TextureCreationRequest);
        }

        public bool Equals(TextureCreationRequest other)
        {
            return ((this.width == other.width)
                   && (this.height == other.height)
                   && (this.pattern == other.pattern));
        }

        public override int GetHashCode()
        {
            return width.GetHashCode()
                 ^ height.GetHashCode()
                 ^ pattern.GetHashCode();
        }

        #endregion
    }
}
