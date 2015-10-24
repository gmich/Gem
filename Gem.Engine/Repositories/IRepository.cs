using Gem.Engine.AI.Promises;
using System;
using System.Collections.Generic;

namespace Gem.Repositories
{
    public interface IRepository<TData>
    {
        IPromise<TData> Load<TId>(TId id);

        IPromise<TData> LoadByPath(string fullPath);

        IPromise<IEnumerable<TData>> LoadAll();

        IPromise SaveRange(IEnumerable<TData> settings);

        IPromise Save(TData settings);

        IPromise Delete<TId>(TId id);
    }
}
