using Gem.Gui.Controls;
using Gem.Gui.Controls.Aggregators;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Elements
{

    public class GuiArea : AGuiComponent, IGuiArea
    {
        private readonly IDictionary<int, IGuiComponent> elements = new Dictionary<int, IGuiComponent>();


        public GuiArea(RenderTemplate renderTemplate,
                             Alignment layoutStyle,
                             Region region,
                             ControlTarget target,
                             IGuiComponent parent = null)
            : base(renderTemplate, layoutStyle, region, target, parent)
        { }

        public int AddComponent(IGuiComponent element)
        {
            int id = elements.Count;
            elements.Add(id, element);

            return id;
        }

        public bool RemoveComponent(int id)
        {
            return elements.Remove(id);
        }

        public IGuiComponent this[int id]
        {
            get
            {
                return elements.ContainsKey(id) ?
                elements[id] : null;
            }
        }

        public int ComponentCount
        {
            get { return elements.Count; }
        }
        public IEnumerable<IGuiComponent> Components()
        {
            foreach (var element in elements.Values)
            {
                yield return element;
            }
        }

    }

}
