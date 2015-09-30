using Gemini.Framework;
using Gemini.Framework.Threading;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Gem.IDE.Infrastructure
{
    public abstract class PersistedEditor : Document, IPersistedDocument
    {
        private bool _isDirty;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (value == _isDirty)
                    return;
                _isDirty = value;
                UpdateDisplayName();
            }
        }

        public bool IsNew
        {
            get { return false; }
        }

        public string FileName
        {
            get; protected set;
        }


        public string FilePath
        {
            get; protected set;
        }


        private void UpdateDisplayName()
        {
            DisplayName = (IsDirty) ? FileName + "*" : FileName;
        }

        public Task New(string fileName)
        {
            return TaskUtility.Completed;
        }

        public Task Load(string filePath)
        {
            return TaskUtility.Completed;
        }

        public async Task Save(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileName(filePath);
            await DoSave(filePath);
            IsDirty = false;
        }

        protected abstract Task DoSave(string filePath);
    }
}
