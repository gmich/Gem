using System;
using System.Collections.Generic;

namespace Gem.Network
{
    /// <summary>
    /// Base class for repositories
    /// </summary>
    /// <typeparam name="TData">The item to store</typeparam>
    /// <typeparam name="TKey">The item's id </typeparam>
    public interface IDataProvider<TData,TKey>
        where TData: class 
    {
        int TotalElements { get; }

        bool HasKey(TKey id);

        TData GetById(TKey id);

        TData Get(Func<TData, bool> expression);

        List<TData> GetAll();

        bool Update(TKey id, TData entity);
 
        bool Delete(TKey id);

        /// <summary>
        /// Add and return the entry as IDisposable.
        /// By disposing, the entry is removed
        /// </summary>
        /// <param name="id">The objects' id</param>
        /// <param name="item">The object to store</param>
        /// <returns>The entry's disposable</returns>
        IDisposable Add(TKey id, TData item);
    }
}

