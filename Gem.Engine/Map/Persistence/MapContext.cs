using System;
using System.IO;

namespace Gem.Map.Persistence
{
    public class MapContext : IDisposable
    {
        private readonly string dbFile;

        public MapContext(string dbName)
        {
            var dbFile = Path.Combine(Path.GetTempPath(), dbName + ".data");
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MapContext()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (File.Exists(dbFile))
            {
                File.Delete(dbFile);
                Console.WriteLine("Deleted main database file");
            }

            if (File.Exists(dbFile + ".pidx"))
            {
                File.Delete(dbFile + ".pidx");
                Console.WriteLine("Deleted primary index file");
            }

            if (File.Exists(dbFile + ".sidx"))
            {
                File.Delete(dbFile + ".sidx");
                Console.WriteLine("Deleted secondary index file");
            }
        }

        #endregion

        public void Do(Action<MapDatabase> databaseAction)
        {
            using (var db = new MapDatabase(dbFile))
            {
                databaseAction(db);
            }
        }
    }
}