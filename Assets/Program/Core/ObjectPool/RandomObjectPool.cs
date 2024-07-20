/*
 *需求是池子里会产生不同种类的预制体，然后 取出的时候随机取一个对象就可以了，不关心是什么类型
 * 
 */
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Logger = Ueels.Core.Debug.Logger;
using Random = UnityEngine.Random;

public class RandomObjectPool<T> :ObjectPool<T>
{
    protected new List<T> pool=new List<T>();

    public override T Depool()
    {
        if(pool.Count==0)
            FillPool();
        
        int selector = Random.Range(0, pool.Count);
        var obj = pool[selector];
        pool[selector] = pool[pool.Count - 1];
        pool.RemoveAt(pool.Count - 1);
        OnDepoolProcessor?.Invoke(obj);
        return obj;
    }

    public override void Enpool(T obj)
    {
        pool.Add(obj);
        OnEnpoolProcessor?.Invoke(obj);
    }
    

}
