using Gem.Gui.Elements;
using System;

namespace Gem.Gui.Input
{
    public class ComponentEnumerator
    {
        private readonly Predicate<IGuiArea> next;
        private readonly Predicate<IGuiArea> previous;

        public event EventHandler<IGuiComponent> onNext;
        public event EventHandler<IGuiComponent> onPrevious;

        public ComponentEnumerator(Predicate<IGuiArea> next, Predicate<IGuiArea> previous)
        {
            this.next = next;
            this.previous = previous;
        }

        private void OnNext(IGuiComponent args)
        {
            var handler = onNext;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPrevious(IGuiComponent args)
        {
            var handler = onPrevious;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void Enumerate(IGuiArea area)
        {
            if (next(area))
            {
                OnNext(area);
            }
            if (previous(area))
            {
                previous(area);
            }
        }
    }
}
