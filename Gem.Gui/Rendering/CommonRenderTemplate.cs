using System;
using System.Collections.Generic;

namespace Gem.Gui.Rendering
{
    public class RenderTemplate
    {
        private readonly RenderStyle style;
        private readonly GuiSprite focused;
        private readonly GuiSprite common;
        private readonly GuiSprite clicked;

        private Dictionary<string, GuiSprite> guiSprites = new Dictionary<string, GuiSprite>();

        public RenderTemplate(RenderStyle style, GuiSprite common, GuiSprite focused, GuiSprite clicked)
        {
            this.style = style;
            this.common = common;
            this.focused = focused;
            this.clicked = clicked;
        }

        public RenderStyle Style { get { return style; } }

        public GuiSprite Common { get { return common; } }

        public GuiSprite Focused { get { return focused; } }

        public GuiSprite Clicked { get { return clicked; } }

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
