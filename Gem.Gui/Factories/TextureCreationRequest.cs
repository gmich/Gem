using Gem.Gui.Styles;
using Microsoft.Xna.Framework;
using System;

namespace Gem.Gui.Factories
{
    internal class TextureCreationRequest : IEquatable<TextureCreationRequest>
    {
        private readonly int width;
        private readonly int height;
        private readonly Color color;
        private readonly IPattern pattern;

        public TextureCreationRequest(int width, int height, Color color, IPattern pattern)
        {
            this.width = width;
            this.height = height;
            this.pattern = pattern;
            this.color = color;
        }

        public int Width { get { return width; } }

        public int Height { get { return height; } }

        public Color Color { get { return color; } }

        public IPattern Pattern { get { return pattern; } }

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
                   && (this.color == other.color)
                   && (this.pattern == other.pattern));
        }

        public override int GetHashCode()
        {
            return width.GetHashCode()
                 ^ height.GetHashCode()
                 ^ pattern.GetHashCode()
                 ^ color.GetHashCode();
        }

        #endregion
    }
}
