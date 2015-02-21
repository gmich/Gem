using System;
using System.Collections.Generic;

namespace Gem.Network
{
    /// <summary>
    /// Base class for repositories
    /// </summary>
    /// <typeparam name="TItem">The item to store</typeparam>
    /// <typeparam name="TId">The item's id </typeparam>
    public interface IDataProvider<TItem,TId>
        where TItem: class 
    {
        bool HasKey(TId id);

        TItem GetById(TId id);

        TItem Get(Func<TItem, bool> expression);

        List<TItem> GetAll();

        bool Update(TItem entity);
 
        bool Delete(TId id);
            
        bool Add(TItem entity);
    }
}

