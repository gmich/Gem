using Gem.Engine.Database;
using System;
using System.IO;
using System.Collections.Generic;

namespace Gem.Map.Persistence
{
    public class MapDatabase : IDatabase<MapModel,Guid>
    {
        readonly Stream mainDatabaseFile;
        readonly Stream primaryIndexFile;
        readonly Stream secondaryIndexFile;
        readonly Tree<Guid, uint> primaryIndex;
        readonly Tree<Tuple<string, int>, uint> secondaryIndex;
        readonly RecordStorage records;
        readonly MapSerializer serializer = new MapSerializer();

        /// <summary>
        /// </summary>
        /// <param name="pathToDb">Path to cow db.</param>
        public MapDatabase(string pathToDb)
        {
            if (pathToDb == null)
                throw new ArgumentNullException("pathToDb");

            // As soon as CowDatabase is constructed, open the steam to talk to the underlying files
            this.mainDatabaseFile = new FileStream(pathToDb, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, Block.Size);
            this.primaryIndexFile = new FileStream(pathToDb + ".pidx", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, Block.Size);
            this.secondaryIndexFile = new FileStream(pathToDb + ".sidx", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, Block.Size);

            // Construct the RecordStorage that use to store main cow data
            this.records = new RecordStorage(new BlockStorage(this.mainDatabaseFile, Block.Size, 48));

            // Construct the primary and secondary indexes 
            this.primaryIndex = new Tree<Guid, uint>(
                new TreeDiskNodeManager<Guid, uint>(
                    new GuidSerializer(),
                    new TreeUIntSerializer(),
                    new RecordStorage(new BlockStorage(this.primaryIndexFile, Block.Size))
                ),
                false
            );

            this.secondaryIndex = new Tree<Tuple<string, int>, uint>(
                new TreeDiskNodeManager<Tuple<string, int>, uint>(
                    new StringIntSerializer(),
                    new TreeUIntSerializer(),
                    new RecordStorage(new BlockStorage(this.secondaryIndexFile, Block.Size))
                ),
                true
            );
        }

        /// <summary>
        /// Update given cow
        /// </summary>
        public void Update(MapModel model)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("MapDatabase");
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert a new cow entry into our cow database
        /// </summary>
        public void Insert(MapModel model)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("MapDatabase");
            }

            // Serialize the cow and insert it
            var recordId = this.records.Create(this.serializer.Serialize(model));

            // Primary index
            this.primaryIndex.Insert(model.Id, recordId);

            // Secondary index
            this.secondaryIndex.Insert(new Tuple<string, int>(model.Name, model.X), recordId);
        }

        /// <summary>
        /// Find a cow by its unique id
        /// </summary>
        public MapModel Find(Guid model)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("CowDatabase");
            }

            // Look in the primary index for this cow
            var entry = this.primaryIndex.Get(model);
            if (entry == null)
            {
                return null;
            }

            return this.serializer.Deserializer(this.records.Find(entry.Item2));
        }

        /// <summary>
        /// Find all cows that belongs to given breed and age
        /// </summary>
        public IEnumerable<MapModel> FindBy(string breed, int age)
        {
            var comparer = Comparer<Tuple<string, int>>.Default;
            var searchKey = new Tuple<string, int>(breed, age);

            // Use the secondary index to find this cow
            foreach (var entry in this.secondaryIndex.LargerThanOrEqualTo(searchKey))
            {
                // As soon as we reached larger key than the key given by client, stop
                if (comparer.Compare(entry.Item1, searchKey) > 0)
                {
                    break;
                }

                // Still in range, yield return
                yield return this.serializer.Deserializer(this.records.Find(entry.Item2));
            }
        }

        /// <summary>
        /// Delete specified cow from our database
        /// </summary>
        public void Delete(MapModel model)
        {
            throw new NotImplementedException();
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                this.mainDatabaseFile.Dispose();
                this.secondaryIndexFile.Dispose();
                this.primaryIndexFile.Dispose();
                this.disposed = true;
            }
        }

        ~MapDatabase()
        {
            Dispose(false);
        }
        #endregion
    }
}
