using System;
using System.Collections.Generic;

namespace Gem.Gui.Rendering
{
    public struct RenderTemplate
    {
        private readonly RenderStyle style;
        private readonly GuiSprite sprite;

        private Dictionary<string, GuiSprite> guiSprites;

        public RenderTemplate(RenderStyle style, GuiSprite sprite)
        {
            this.style = style;
            this.sprite = sprite;
            guiSprites = new Dictionary<string, GuiSprite>();
            guiSprites.Add("Default", sprite);
        }

        public RenderStyle Style { get { return style; } }

        public GuiSprite Sprite { get { return sprite; } }

        public GuiSprite this[string spriteId]
        {
            get
            {
                return guiSprites.ContainsKey(spriteId) ?
                       guiSprites[spriteId] : null;
            }
        }

        public RenderTemplate AddGuiSprite(string key, GuiSprite guiSprite)
        {
            if (guiSprites.ContainsKey(key))
            {
                throw new ArgumentException("This key is already registered in the guisprite dictionary");
            }

            guiSprites.Add(key, guiSprite);
            return this;
        }

    }
}
