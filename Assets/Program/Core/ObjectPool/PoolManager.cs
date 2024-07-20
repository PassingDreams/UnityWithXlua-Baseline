using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Ueels.Core.Debug;
using UnityEngine;
using Logger = Ueels.Core.Debug.Logger;

namespace Ueels
{
    
    /// <summary>
    /// 同类对象共用一个池，故不支持Gameobject Prefab
    /// </summary>
    public class PoolManager
    {
        private static Hashtable poolList=new Hashtable();

        static PoolManager()
        {
            Init();
        }
        private static void Init()
        {
            AddNewPool(() => { return new StringBuilder(); });
        }


        private static string ClassName<T>()
        {
            Type t=typeof(T);
            return  t.ToString();
        }
        
        private static ObjectPool<T> FindPool<T>()
        {

            string typeName = ClassName<T>();
            if (!poolList.Contains(typeName))
                ThrowHelper.Throw("Pool not build");
            return (ObjectPool<T>) poolList[typeName];
        }

        private static bool ContainsPool<T>()
        {
            return poolList.Contains(ClassName<T>());
        }

        public static ObjectPool<T> GetPool<T>()
        {
            return FindPool<T>();
        }
        public static T Depool<T>()
        {
            return FindPool<T>().Depool();
        }
        
        public static void Enpool<T>(T obj)
        {
            FindPool<T>().Enpool(obj);
        }

        public static ObjectPool<T> AddNewPool<T>(Func<T> objConstructor)
        {
            string typeName = ClassName<T>();
            if (poolList.Contains(typeName))
            {
                Logger.PrintError("该类型池已被创建过");
                return GetPool<T>();
            }
            
            var pool=new ObjectPool<T>(objConstructor,typeName);
            poolList.Add(typeName,pool);
            return pool;
        }
        
        public static void AddNewPool<T>(ObjectPool<T> pool)
        {
            string typeName = ClassName<T>();
            if (poolList.Contains(typeName))
            {
                Logger.PrintError("该类型池已被创建过");
                return;
            }
            
            poolList.Add(typeName,pool);
        }

    }

}
