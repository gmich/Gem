using System;
using System.Collections.Generic;

namespace Gem.Infrastructure
{
    /// <summary>
    /// Helper class for disposable collection entries.
    /// Disposing removes the entry from the list
    /// <remarks>Not thread safe</remarks>
    /// </summary>
    /// <typeparam name="TEntry">The IList's generic type</typeparam>
    public sealed class DisposableEntry<TEntry> : IDisposable
    {
        private IList<TEntry> registered;
        private TEntry current;

        internal DisposableEntry(IList<TEntry> registered, TEntry current)
        {
            this.registered = registered;
            this.current = current;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool isDisposed = false;
        private void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                if (registered.Contains(current))
                    registered.Remove(current);
                isDisposed = true;
            }
        }
    }

    /// <summary>
    /// Factory for creating disposable entries
    /// </summary>
    public static class Disposable 
    {
        public static DisposableEntry<TEntry> Create<TEntry>(IList<TEntry> registered, TEntry current)
        {
            return new DisposableEntry<TEntry>(registered, current);
        }
        public static IDisposable CreateDummy()
        {
            return new DummyDisposable();
        }
    }

    public class DummyDisposable : IDisposable
    {
        public void Dispose()
        {
            return;
        }
    }
}
