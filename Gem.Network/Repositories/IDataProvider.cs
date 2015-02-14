using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network
{

    public interface IDataProvider<TItem,TId>
        where TItem: class 
        where TId : struct
    {
        bool HasKey(TId id);

        TItem GetById(TId id);

        TItem Get(Func<TItem, bool> expression);

        bool Update(TItem entity);
 
        bool Delete(TId id);
            
        bool Add(TItem entity);
    }
}

