using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack<T>
{
    private List<T> buf;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="capacity">-1即默认容量</param>
    public Stack(int capacity=-1)
    {
        if(capacity<0)
            buf=new List<T>();
        else
        {
            buf=new List<T>(capacity);
        }
    }

    public void Push(T item)
    {
        buf.Add(item);
    }

    public T Pop()
    {
        var item = buf[buf.Count - 1];
        buf.RemoveAt(buf.Count-1);
        return item;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idx">-1默认为查看栈顶元素</param>
    /// <returns></returns>
    public T Peep(int idx=-1)
    {
        if (idx < 0)
        {
            return buf[buf.Count - 1];
        }
        return buf[idx];
    }

    public bool IsEmpty()
    {
        return buf.Count == 0;
    }

    public void Clear()
    {
        buf.Clear();
    }
}
