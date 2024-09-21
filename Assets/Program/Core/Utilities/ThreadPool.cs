using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Ueels.Core;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ThreadPool : Ueels.Core.Singleton<ThreadPool>
{
    private List<Thread> pool=new List<Thread>();
    public Thread GetThread(Action<object> func) //启动可带一个参数对象
    {
        ParameterizedThreadStart start = (obj) =>
        {
            func(obj);
        };
        Thread runner=new Thread(start);
        pool.Add(runner);
        return runner;
    }

    public void StopAll()
    {
        foreach (var thread in pool)
        {
            thread?.Abort();
        }
        Debug.Log("终结线程个数："+pool.Count);
        pool.Clear();
    }
    
}
