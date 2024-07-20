
using System;
using System.Collections.Generic;
using Ueels.Core.Debug;


namespace Ueels
{
    
    //默认大的优先
	//注意这里的接口是YH命名空间下的接口
    public class PriorityQueue <T> where T: IComparable
    {

        private MaxHeap<T> heap;

        public PriorityQueue(int capacity)
        {
            heap=new MaxHeap<T>(capacity);
        }

        public void Enqueue(T elem)
        {
            heap.HeapAdd(elem);
        }

        /// <summary>
        /// 出队最大元素并保持堆性质
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            return heap.HeapPop(0);
        }

        /// <summary>
        /// 移除任意位置元素并保持堆性质
        /// </summary>
        /// <param name="elm"></param>
        /// <param name="matchMethod"></param>
        /// <returns></returns>
        public T Remove(Predicate<T> matchMethod)
        {
            if (matchMethod == null)
            {
                Logger.PrintError("ERR! No match method set");
                
            }

            return heap.HeapPop(heap.FindIndex(matchMethod));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputBuf">用于提供输出缓冲</param>
        /// <param name="isUpOrderDump">是否需要按优先级高的放在数组的后面输出</param>
        public void DumpAndClear(List<T> outputBuf, bool isUpOrderDump = false)
        {
            //是升序提取，权限越高越往后
            if(isUpOrderDump)
                foreach (var elm in heap.UpOrderDump())
                {
                    outputBuf.Add(elm);
                }
            else
            {
                var dump = heap.UpOrderDump();
                for (int i = dump.Count - 1; i >= 0; i--)
                {
                    outputBuf.Add(dump[i]);
                }
            }
            heap.Clear();
        }

        public void Clear()
        {
            heap.Clear();
        }

        public bool IsEmpty()
        {
            return heap.IsEmpty();
        }


        public static void _UseCase_TestDemo()
        {
            PriorityQueue<int> priorityQueue=new PriorityQueue<int>(5);
            priorityQueue.Enqueue(5);
            priorityQueue.Enqueue(2);
            priorityQueue.Enqueue(3);
            priorityQueue.Enqueue(1);
            priorityQueue.Enqueue(8);
            
            priorityQueue.Remove( (t) => { return t == 3; });

            Logger.PrintHint(priorityQueue.Dequeue());
            Logger.PrintHint(priorityQueue.Dequeue());
            Logger.PrintHint(priorityQueue.Dequeue());
            Logger.PrintHint(priorityQueue.Dequeue());

        }
        
    }

}
