using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.Database
{
    public interface IDatabase<TModel,TKey> : IDisposable
    {
        void Insert(TModel model);
        void Delete(TModel model);
        void Update(TModel model);
        TModel Find(TKey id);
    }
}
