using System;
using System.Threading.Tasks;

namespace Gem.Repositories
{
    public static class AsyncRepositoryWrapper
    {
        public static TResult ExecuteAsync<TData, TResult>(
                                                this IRepository<TData> repository, 
                                                Func<IRepository<TData>, TResult> asyncAction)
        {
           return AsyncHelper(repository, asyncAction).Result;
        }

        private static async Task<TResult> AsyncHelper<TData, TResult>(
                                                IRepository<TData> repository,
                                                Func<IRepository<TData>, TResult> asyncAction)
        {
            return await Task<TResult>.Run(() => asyncAction(repository));
        }
    }
}
