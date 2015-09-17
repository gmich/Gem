using Gem.AI.Promises;
using System.Collections.Generic;

namespace Gem.Repositories
{
    public interface IRepository<TData>
    {
        IPromise<IEnumerable<TData>> LoadAll();

        IPromise SaveRange(IEnumerable<TData> settings);

        IPromise Save(TData settings);
    }
}
