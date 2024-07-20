using System.Collections;
using System.Collections.Generic;

namespace Ueels
{
    
    /// <summary>
    /// Double-ended queue
    /// </summary>
    public class Deque<T> : IEnumerable<T>
    {
        private LinkedList<T> datas;

        public Deque()
        {
            datas=new LinkedList<T>();
        }
        
        public void PushBack(T elm)
        {
            datas.AddLast(elm);
        }

        public void PushFront(T elm)
        {
            datas.AddFirst(elm);
        }

        public T PopFront()
        {
            var data = datas.First.Value;
            datas.RemoveFirst();
            return data;
        }
        
        public T PopBack()
        {
            var data = datas.Last.Value;
            datas.RemoveLast();
            return data;
        }

        public int Count => datas.Count;
        public bool IsEmpty => Count == 0;

        public void Clear()
        {
            Queue<int> t=new Queue<int>();
            datas.Clear();
        }
        
        
        
        
        /*----------------------enumerable---------------------*/

        public IEnumerator<T> GetEnumerator()
        {
            return datas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

