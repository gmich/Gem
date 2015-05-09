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

            public SpriteItem(Texture2D texture, Rectangle? sourceRectangle = null)
            {
                this.texture = texture;
                this.sourceRectangle = sourceRectangle;
            }

            public Texture2D Texture { get { return texture; } }

            public Rectangle? SourceRectangle { get { return sourceRectangle; } }
        }

        #endregion

        #region Ctor

        public Sprite(Texture2D texture, Rectangle? sourceRectangle = null)
        {
            this.spriteContainer = new AContainer<SpriteItem>();
            this.Add(target, texture, sourceRectangle);
        }

        #endregion

        #region Properties

        public Texture2D Texture
        {
            get { return spriteContainer[target].Texture; }
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

        public bool Add(string spriteId, Texture2D texture, Rectangle? sourceRectangle = null)
        {
            return spriteContainer.Add(spriteId, new SpriteItem(texture, sourceRectangle));
        }

        #endregion

    }
}
