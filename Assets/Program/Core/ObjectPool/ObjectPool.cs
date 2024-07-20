using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Logger = Ueels.Core.Debug.Logger;

//需要传入一个无参构造方法后使用
//2023/1/9:更新了计数器
public class ObjectPool<T>
{
    protected Queue<T> pool=new Queue<T>();
    public int Count => pool.Count;

    //processors
    protected Func<T> InstantiateMethod = null;//Func的最后一个泛型参数即返回值
    protected Action<T> OnFillpoolProcessor = null;
    protected Action<T> OnDepoolProcessor = null;
    protected Action<T> OnEnpoolProcessor = null;
    protected string poolName;
    protected int objAllocateCounter = 0;


    /// <summary>
    /// 无参构造函数不进行对象初始化，需要手动填充Fill或懒加载
    /// </summary>
    /// <param name="poolName"></param>
    public ObjectPool(string poolName="")
    {
        this.poolName = poolName;
    }

    /// <summary>
    /// 填入对象生成方法的池会自动初始化，生成一批对象
    /// </summary>
    /// <param name="objConstructer"></param>
    public ObjectPool(Func<T> objConstructer,string poolName="",bool isLazyLoad=true)
    {
        SetObjConstructer(objConstructer);
        this.poolName = poolName;
        InitializePoolLoading(isLazyLoad);
    }
    public ObjectPool(Func<T> objConstructer,Action<T> onEnpoolProcessor,string poolName="",bool isLazyLoad=true)
    {
        SetObjConstructer(objConstructer);
        SetEnpoolProcessor(onEnpoolProcessor);
        this.poolName = poolName;
        InitializePoolLoading(isLazyLoad);
    }

    protected virtual void InitializePoolLoading(bool isLazyLoad)
    {
        if(!isLazyLoad)
            FillPool();
    }

    /*-------------------------填充设置-----------------------*/

    private int increment=-1;
    public int AutoFillPoolIncrement
    {
        get
        {
            if (increment < 0)
            {
                increment = 5;//默认增量
            }
            return increment;
        }
        set
        {
            increment = value;
            if (increment > 100)
            {
                Logger.PrintWarning("too large increment, maybe waste space");
            }
        }
        
        
    }
    public  void FillPool()
    {
        FillPool(AutoFillPoolIncrement);
    }

    public void FillPool(int onceIncrement)
    {
        if (InstantiateMethod == null)
        {
            Debug.Log("Err: "+typeof(T).Name+"has no Obj Constructer!");
            return;
        }
        
        for (int i = 0; i < onceIncrement; i++)
        {
            var obj = InstantiateMethod();
            objAllocateCounter++;
            OnFillpoolProcessor?.Invoke(obj);
            Enpool(obj);
        }
        Logger.PrintHint("current totally allocate objs of <"+poolName+"> is: "+ objAllocateCounter);
        
    }

    public virtual T Depool()
    {
        if(pool.Count==0)
            FillPool();
        
        var obj= pool.Dequeue();
        OnDepoolProcessor?.Invoke(obj);
        return obj;
    }

    public virtual void Enpool(T obj)
    {
        pool.Enqueue(obj);
        OnEnpoolProcessor?.Invoke(obj);
    }
    
    public void SetPoolName(string poolName)
    {
        this.poolName = poolName;
    }

    public void SetObjConstructer(Func<T> constructer)
    {
        InstantiateMethod = constructer;
    }
    
    //这一组processor一般供实例来自定义和调用
    public void SetEnpoolProcessor(Action<T> func)
    {
        OnEnpoolProcessor = func;
    }
    public void SetDepoolProcessor(Action<T> func)
    {
        OnDepoolProcessor = func;
    }

    
    
    
    
    
    /*------------------------------extend-------------------*/
    //因为组件池比较常用，所以我在这里提供了简单的工厂方法（当然也可以视作使用案例）
    private static void GameobjectActivate(Component component)
    {
        component.gameObject.SetActive(true);
    }
    private static void GameobjectDeActivate(Component component)
    {
        component.gameObject.SetActive(false);
        component.transform.SetParent(MonoHanger);//如果一个池对象被挂到移动物体身上，那么再次激活后就会被错误带动，所以必须归池置空
    }
    private static Transform MonoHanger
    {
        get
        {
            if (objectHanger != null) return objectHanger.transform;

            objectHanger = new GameObject($"PoolObjectHind[{typeof(T)}]");
            objectHanger.transform.position=Vector3.zero;
            return objectHanger.transform;
        }
    }
    private static GameObject objectHanger = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pref">monobehavior预制体</param>
    /// <param name="hanger">挂载点transform</param>
    /// <param name="poolName"></param>
    /// <typeparam name="TC">继承自Component的对象</typeparam>
    /// <returns></returns>
    public static ObjectPool<TC> BuildComponentPool<TC>(GameObject pref,Transform hanger,string poolName) where TC : Component
    {
        ObjectPool<TC> pool=new ObjectPool<TC>(poolName);
        pool.SetObjConstructer(() =>
        {
            var go = GameObject.Instantiate(pref, hanger, true);
            return go.GetComponent<TC>();
        });
        pool.SetEnpoolProcessor(GameobjectDeActivate);
        pool.SetDepoolProcessor(GameobjectActivate);
        pool.FillPool();
        return pool;
    }
    
    
    
    
    
    


}
