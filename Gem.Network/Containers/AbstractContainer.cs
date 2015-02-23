using Gem.Network.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Network.Containers
{
    public class AbstractContainer<TData, TKey>
        where TData : class
    {
        private readonly IDataProvider<TData, TKey> dataRepository;

        public AbstractContainer(IDataProvider<TData, TKey> dataRepository)
        {
            this.dataRepository = dataRepository;
        }
        
        public void AddOrUpdate(TData data,TKey key)
        {
            if(dataRepository.HasKey(key))
            {
                dataRepository.Update(key, data);
            }
            else
            {
                dataRepository.Add(key, data);
            }
        }

        public IEnumerable<TData> GetAll()
        {
            return dataRepository.GetAll();
        }

        public TData this[TKey id]
        {
            get
            {
                return dataRepository.GetById(id);
            }
        }
    }       
}
