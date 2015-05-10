using Gem.Gui.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{
    public class Sprite
    {
        #region Fields

        private readonly AContainer<SpriteItem> spriteContainer;
        private string target = "default";

        #endregion

        #region Item

        private class SpriteItem
        {
            private readonly Texture2D texture;
            private readonly Rectangle? sourceRectangle;
            private readonly Vector2 origin;

            public SpriteItem(Texture2D texture, Vector2? origin = null, Rectangle? sourceRectangle = null)
            {
                this.texture = texture;
                this.sourceRectangle = sourceRectangle;
                this.origin = origin ?? Vector2.Zero;
            }

            public Vector2 Origin { get { return origin; } }

            public Texture2D Texture { get { return texture; } }

            public Rectangle? SourceRectangle { get { return sourceRectangle; } }
        }

        #endregion

        #region Ctor

        public Sprite(Texture2D texture, Vector2? origin = null, Rectangle? sourceRectangle = null)
        {
            this.spriteContainer = new AContainer<SpriteItem>();
            this.Add(target, texture,origin, sourceRectangle);
        }

        #endregion

        #region Properties

        public Texture2D Texture
        {
            get { return spriteContainer[target].Texture; }
        }

        public Vector2 Origin
        {
            get { return spriteContainer[target].Origin; }
        }

        public Rectangle? SourceRectangle
        {
            get { return spriteContainer[target].SourceRectangle; }
        }
        #endregion

        #region Public Helper Methods

        public bool SwitchSprite(string spriteId = "default")
        {
            if (spriteContainer.Has(spriteId))
            {
                target = spriteId;
                return true;
            }
            return false;
        }

        public bool Add(string spriteId, Texture2D texture, Vector2? origin = null, Rectangle? sourceRectangle = null)
        {
            return spriteContainer.Add(spriteId, new SpriteItem(texture, origin, sourceRectangle));
        }

        #endregion

    }
}
