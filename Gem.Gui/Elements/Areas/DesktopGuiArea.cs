using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Elements
{

    public class DesktopGuiArea<TEventArgs> : DesktopElement<TEventArgs>, IGuiArea
        where TEventArgs : EventArgs
    {
        private readonly IDictionary<int, IGuiElement> elements = new Dictionary<int, IGuiElement>();

        public DesktopGuiArea(RenderTemplate renderTemplate,
                             GuiSprite mouseCaptureSprite,
                             LayoutStyle layoutStyle,
                             Region region,
                             Func<IGuiElement, TEventArgs> eventArgsProvider,
                             int order = 0,
                             IGuiElement parent = null)
        :base(renderTemplate, mouseCaptureSprite, layoutStyle, region, eventArgsProvider, order, parent)
        { }

        public int AddElement(IGuiElement element)
        {
            int id = elements.Count;
            elements.Add(id, element);

            return id;
        }

        public bool RemoveElement(int id)
        {
            return elements.Remove(id);
        }

        public IGuiElement this[int id]
        {
            get
            {
                return elements.ContainsKey(id) ?
                elements[id] : null;
            }
        }

        public IEnumerable<IGuiElement> Elements()
        {
            foreach (var element in elements.Values)
            {
                yield return element;
            }
        }
    }

}
