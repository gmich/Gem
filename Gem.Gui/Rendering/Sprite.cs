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
            private readonly Vector2 center;

            public SpriteItem(Texture2D texture, Rectangle? sourceRectangle, Vector2 center)
            {
                this.texture = texture;
                this.sourceRectangle = sourceRectangle;
                this.center = center;
            }

            public Texture2D Texture { get { return texture; } }

            public Rectangle? SourceRectangle { get { return sourceRectangle; } }

            public Vector2 Center { get { return center; } }
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

        public Vector2 Center
        {
            get { return spriteContainer[target].Center; }
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

        public bool Add(string spriteId, Texture2D texture, Rectangle? sourceRectangle = null, Vector2? center = null)
        {
            return spriteContainer.Add(spriteId,
                                       new SpriteItem(texture,
                                                      sourceRectangle,
                                                      center ?? new Vector2(texture.Width / 2, texture.Height / 2)));
        }

        #endregion

    }
}
