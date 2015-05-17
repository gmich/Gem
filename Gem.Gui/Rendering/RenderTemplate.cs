using System;
using Gem.Gui.Fluent;

namespace Gem.Gui.Rendering
{
    public class RenderTemplate
    {
        private readonly IControlDrawable controlDrawable;
        private readonly ITextDrawable textDrawable;

        public RenderTemplate(IControlDrawable controlDrawable,ITextDrawable textDrawable)
        {
            this.controlDrawable = controlDrawable;
            this.textDrawable = textDrawable;
        }

        public IControlDrawable ControlDrawable { get { return controlDrawable; } }

        public ITextDrawable TextDrawable { get { return textDrawable; } }

        private static Lazy<RenderTemplate> _defaultTemplate = 
                   new Lazy<RenderTemplate>(() => new RenderTemplate(RenderControlBy.Frame, RenderTextBy.Position));

        public static RenderTemplate Default
        {
            get
            {
                return _defaultTemplate.Value;
            }
        }
    }
}
