using System;
using System.Collections.Generic;

namespace Gem.Console
{
    public class FixedSizeList<T>
    {
        private readonly List<T> list = new List<T>();
        private readonly int limit;

        public FixedSizeList(int limit)
        {
            if (limit < 0)
            {
                throw new ArgumentException("limit");
            }
            this.limit = limit;
        }

        public IEnumerable<TResult> Query<TResult>(Func<List<T>, IEnumerable<TResult>> query)
        {
            return query(list);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public T this[int index]
        {
            get
            {
                return list[index];
            }
        }

        public IEnumerable<T> Collection
        {
            get { return list; }
        }

        public void Add(T obj)
        {
            list.Add(obj);
            GuardSize();
        }

        private void GuardSize()
        {
            lock (this)
            {
                while (list.Count > limit)
                {
                    list.RemoveAt(0);
                }
            }
        }

    }
}
