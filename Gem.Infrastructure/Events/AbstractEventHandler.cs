using System;
using System.Collections.Generic;

namespace Gem.Infrastructure
{
    public abstract class AbstractEventHandler<TEventArgs>
        where TEventArgs : EventArgs, new()
    {

        private readonly Dictionary<string, EventHandler<TEventArgs>> Handlers =
                     new Dictionary<string, EventHandler<TEventArgs>>();

        public void Add(string system, EventHandler<TEventArgs> ev)
        {
            if (Handlers.ContainsKey(system))
            {
                Handlers[system] += ev;
            }
            else
            {
                Handlers.Add(system, ev);
            }
        }

        public void Remove(string system, EventHandler<TEventArgs> ev)
        {
            Handlers[system] -= ev;
        }

        public void Fire(string system, TEventArgs args)
        {
            if (string.IsNullOrEmpty(system))
            {
                EventHandler<TEventArgs> handler = null;
                Handlers.TryGetValue(system, out handler);

                if (handler != null)
                {
                    handler(null, args);
                }

                handler = Handlers[""] as EventHandler<TEventArgs>;
                if (handler != null)
                {
                    handler(null, args);
                }
            }
        }
    }
}

