using Gem.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace Gem.Network.Server
{
    /// <summary>
    /// Registers / Deregisters objects
    /// </summary>
    public class Registerer<T> : IDisposable
        where T: class
    {

        #region Fields / Properties

        public List<T> Registered { get; private set; }

        private Dictionary<string, IDisposable> disposables;

        private readonly int maxRegistrations;

        private bool isDisposed;

        private bool AllowRegistration
        {
            get
            {
                return Registered.Count < maxRegistrations;
            }
        }

        #endregion


        #region Construct / Dispose

        public Registerer(int maxRegistrations)
        {
            this.maxRegistrations = maxRegistrations;
            Registered = new List<T>();  
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    foreach (var disposable in disposables)
                    {
                        disposable.Value.Dispose();
                    }
                    disposables = null;
                    Registered = null;
                }
                this.isDisposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
        #endregion

        /// <summary>
        /// Registers an object and returns it's disposable
        /// </summary>
        /// <param name="obj">The object to register</param>
        /// <returns>The registered object's disposable</returns>
        public IDisposable Register(T obj)
        {
            if (AllowRegistration)
            {
                Registered.Add(obj);
                return new DeregisterDisposable<T>(Registered, obj);
            }
            return null;
        }

        /// <summary>
        /// Registers a new instance. 
        /// The instance's disposable is put in a dictionary with its tag.
        /// </summary>
        /// <param name="tag">The disposable's tag</param>
        /// <param name="obj">The object to register</param>
        /// <returns>True for success</returns>
        public bool Register(string tag, T obj)
        {
            var disposable = Register(obj);

            if (disposable == null || disposables.ContainsKey(tag))
            {
                return false;
            }

            disposables.Add(tag, disposable);
            return true;
        }

        /// <summary>
        /// Deregisters / Disposes an object
        /// </summary>
        /// <param name="tag">The object's tag</param>
        /// <returns>True for success</returns>
        public bool Deregister(string tag)
        {
            if (disposables.ContainsKey(tag))
            {
                disposables[tag].Dispose();
                return true;
            }

            return false;
        }

        internal class DeregisterDisposable<T> : IDisposable
        {
            private List<T> registered;
            private T current;

            internal DeregisterDisposable(List<T> registered, T current)
            {
                this.registered = registered;
                this.current = current;
            }

            public void Dispose()
            {
                if (registered.Contains(current))
                    registered.Remove(current);
            }
        }

    }
}
