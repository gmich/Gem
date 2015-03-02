using Gem.Network.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Network.Containers
{
    public abstract class AbstractContainer<TData, TKey>
        where TData : class,new()
    {
        protected readonly IDataProvider<TData, TKey> dataRepository;

        public AbstractContainer(IDataProvider<TData, TKey> dataRepository)
        {
            this.dataRepository = dataRepository;
        }
        
        public bool Update(TData data,TKey key)
        {
            if(dataRepository.HasKey(key))
            {
                return dataRepository.Update(key, data);
            }
            return false;
        }

        public IEnumerable<TData> GetAll()
        {
            return dataRepository.GetAll();
        }

        public bool HasKey(TKey id)
        {
            return dataRepository.HasKey(id);
        }

        public virtual TData this[TKey id]
        {
            get
            {
                if (dataRepository.HasKey(id))
                {
                    return dataRepository.GetById(id);
                }
                else
                {
                    var newData = new TData();
                    dataRepository.Add(id, newData);
                    return newData;
                }               
            }           
        }
    }       
}
