using System.Collections.Generic;

namespace Gem.Network.Containers
{
    /// <summary>
    /// Readonly container for storing objects and retreiving them by key
    /// </summary>
    /// <typeparam name="TData">The object to store</typeparam>
    /// <typeparam name="TKey">The key</typeparam>
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
