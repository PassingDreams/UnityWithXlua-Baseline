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
    public Thread GetThread(Action func)
    {
        ThreadStart start = () =>
        {
            func();
        };
        Thread runner=new Thread(start);
        runner.Start();
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
