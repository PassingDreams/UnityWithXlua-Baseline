using System;
using System.Collections;
using System.Collections.Generic;
using Ueels.Core.Debug;
using UnityEngine;

public interface IComponent
{
    ComponentContainer Carrier { get; set; }//主体
    
    /// <summary>
    ///回收成员空间并将自身放入对象池 
    /// </summary>
    void DestroySelf();
} 
public abstract class ComponentContainer
{
    protected List<IComponent> componentList;

    public void AddComponent(IComponent component)
    {
        if (componentList == null)
        {
            componentList = AllocateComponentsBuffer();
            componentList.Clear();
        }

        component.Carrier = this;
        componentList.Add(component);
    }

    public  T GetComponent<T>() where T : class, IComponent
    {
        foreach (var icomponent in componentList)
        {
            if (icomponent is T componentT)
            {
                return componentT;
            }
            
        }
        return null;
    }

    /// <summary>
    /// 释放(或入池)所有组件，并释放存放组件指针的buffer
    /// </summary>
    public void DestroyComponentsAndBuffer()
    {
        if (componentList != null)
        {
            foreach (var component in componentList)
            {
                component.DestroySelf();
            }
            componentList.Clear();
            RecycleComponentsBuffer(componentList);
        }
    }

    
    /// <summary>
    /// 申请内存方法
    /// </summary>
    /// <returns></returns>
    public abstract List<IComponent> AllocateComponentsBuffer();
    /// <summary>
    /// 回收内存方法
    /// </summary>
    /// <param name="list"></param>
    public abstract void RecycleComponentsBuffer(List<IComponent> list);
}


/*-----------------------*/
//由于不支持多重继承，权衡之下只能采取这种方式
public abstract class NoneableComponentContainer<T,E>: ComponentContainer where E:Enum where T:NoneableComponentContainer<T,E>,new() 
    
{
        public E Type
        {
            get { return _type; }
        }

        private E _type;//禁止外部直接设置类型
        
        
        //空对象唯一实例
        public static T None 
        {
            get
            {
                if(_none==null)
                    BuildSingleNone();

                return _none;
            }
        }
        private static T _none = default;

        
        /// <summary>
        /// 构建唯一空实例,这个函数最多只会被调用一次
        /// </summary>
        /// <returns>返回空实例</returns>
        private static void BuildSingleNone()
        {
            _none=new T();
            _none._type = _none.NoneType();
        }
        
        /// <summary>
        /// 用来告知基类空类None class对应的枚举类型
        /// </summary>
        /// <returns></returns>
        protected abstract E NoneType();
        
        
        //可以开放设置的类型,此处负责封闭None的构建
        public void SetType(E type)
        {
            if (type.Equals(NoneType()))
            {
                ThrowHelper.Throw("禁止设置空类型，空类型应在全局具有唯一的实例");
            }
            _type = type;
        }
}


    
