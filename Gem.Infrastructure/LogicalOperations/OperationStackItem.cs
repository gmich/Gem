using System;

namespace Gem.Infrastructure.LogicalOperations
{
    public sealed class OperationStackItem : IDisposable
    {
        #region Fields

        private readonly object operation;
        private readonly int depth;
        private OperationStackItem parent = null;
        private bool disposed = false;

        #endregion

        #region Ctor

        internal OperationStackItem(OperationStackItem parentOperation, object operation)
        {
            parent = parentOperation;
            this.operation = operation;
            depth = parent == null ? 1 : parent.Depth + 1;
        }

        #endregion

        #region Properties

        internal object Operation { get { return operation; } }
        internal int Depth { get { return depth; } }

        internal OperationStackItem Parent { get { return parent; } }

        #endregion

        public override string ToString()
        {
            return operation != null ? operation.ToString() : "";
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                if (disposed) return;

                OperationStack.Pop();

                disposed = true;
            }
        }

        #endregion
    }
}