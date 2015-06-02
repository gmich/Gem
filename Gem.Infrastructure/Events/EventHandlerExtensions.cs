using System;

namespace Gem.Infrastructure.Events
{
    public static class EventHandlerExtensions
    {
        public static void RaiseEvent<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs args)
        {
            var safeHandler = handler;
            if (safeHandler != null)
            {
                safeHandler(sender, args);
            }
        }
    }
}
