using System;
using System.Collections.Generic;

namespace Ueels
{

    public abstract class Heap<T> where T: IComparable//这个接口要求结果返回t1-t2
    {
        protected List<T> buf;
        public int Count => buf.Count;

        public Heap(int capacity)
        {
            buf=new List<T>(capacity);
        }
        

        /// <summary>
        /// 不破坏堆性质的元素增加
        /// </summary>
        /// <param name="obj"></param>
        public virtual void HeapAdd(T obj)
        {
        }

        public void Clear()
        {
            buf.Clear();
        }

        public bool IsEmpty()
        {
            return buf.Count == 0;
        }

        public T Last()
        {
            return buf[buf.Count-1];
        }

        public int FindIndex(Predicate<T> matchMethod)//找到具有匹配该条件的元素索引
        {
            return buf.FindIndex(matchMethod);
        }


        /// <summary>
        /// 不破坏堆性质的元素移除
        /// </summary>
        /// <param name="index"></param>
        public void HeapRemove(int index)
        {
            HeapPop(index);
        }
        /// <summary>
        /// 不破坏堆性质的元素pop
        /// </summary>
        /// <param name="index"></param>
        public abstract T HeapPop(int index);
        

        public static int GetLeftSonIndexOf(int father)
        {
            return father * 2 + 1;
        }

        public static int GetFatherIndexOf(int son)
        {
            return (son + 1) / 2 - 1;
        }

    }

    
    
    public class MaxHeap<T>: Heap<T> where T : IComparable
    {
        public MaxHeap(int capacity):base(capacity){}
        //重新构造整个buffer成为大顶堆
        public void MaxHeapfy()
        {
            //make max heap
            for(int i=buf.Count/2-1;i>=0;i--)
            {
                PressNodeDownToBottom(i);
            }
        }

        public void PressNodeDownToBottom(int idx)//节点下沉调整
        {
            T t = buf[idx];
            int i,anch;
            for( i=idx,anch=idx;i<buf.Count;i=anch)
            {
                anch = GetLeftSonIndexOf(anch);
                if(anch+1<buf.Count && buf[anch+1].CompareTo(buf[anch])>0)//anch to the bigger brother
                    anch+=1;
                if(anch<buf.Count && buf[anch].CompareTo(t)>0)//bigger son go to small father
                    buf[i]=buf[anch];
                else
                    break;
            }
            buf[i]=t;//small ancestor go to suitable position
        }

        /// <summary>
        /// 节点上浮调整,用于优先队列数据插入
        /// </summary>
        /// <param name="idx"></param>
        public void LiftNodeUpToTop(int idx)
        {
            T t = buf[idx];
            int i,anch;
            for( i=idx,anch=idx;i>=0;i=anch)
            {
                anch = GetFatherIndexOf(anch);
                if (anch >= 0 && t.CompareTo(buf[anch])>0)//在基本有序的堆中，如果一个插入节点比父亲大，那么它一定比兄弟大
                    buf[i] = buf[anch];
                else
                    break;
            }
            buf[i] = t;
        }

        public override void HeapAdd(T obj)//实现方法：将元素加到buf尾，并上浮调整
        {
            buf.Add(obj);
            LiftNodeUpToTop(buf.Count-1);
        }

        public override T HeapPop(int index)//实现方法：将队尾元素插入弹出位置,如果比父亲大就上升，否则就下沉
        {
            var res = buf[index];
            if (index == buf.Count - 1)//如果恰好是队尾，那么就直接弹出返回
            {
                buf.RemoveAt(Count-1);
                return res;
            }
            buf[index] = Last();
            buf.RemoveAt(Count-1);


            var fatherIdx = GetFatherIndexOf(index);
            if(fatherIdx>=0&&buf[index].CompareTo(buf[fatherIdx])>0)
                LiftNodeUpToTop(index);
            else
                PressNodeDownToBottom(index);
            return res;
        }

        
        

        
        /// <summary>
        /// 根部下沉，但是额外限定了宽度，用于堆排序
        /// </summary>
        /// <param name="root"></param>
        /// <param name="maxLen"></param>
        private void AdjustHeap(int root,int maxLen)
        {
            T t = buf[root];
            int i,anch;
            for( i=root,anch=root;i<maxLen;i=anch)
            {
                anch = GetLeftSonIndexOf(anch);
                if(anch+1<maxLen && buf[anch+1].CompareTo(buf[anch])>0)//anch to the bigger brother
                    anch+=1;
                if(anch<maxLen && buf[anch].CompareTo(t)>0)//bigger son go to small father
                    buf[i]=buf[anch];
                else
                    break;
            }
            buf[i]=t;//small ancestor go to suitable position
        }
        
        /// <summary>
        /// 按升序返回buffer,警告：调用此方法会破坏大顶堆的性质，所以这个方法用后应清空堆
        /// </summary>
        /// <returns></returns>
        public List<T> UpOrderDump()
        {
            MaxHeapfy();
            //get max elem at tail, heap size shrinked
            for(int i=buf.Count-1;i>0;i--)
            {
                var t=buf[i];
                buf[i]=buf[0];
                buf[0]=t;
                AdjustHeap(0,i);
            }
            return buf;
        }
    }

}
