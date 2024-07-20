using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 遍历模式，通过一定方式遍历给定区域
/// 2D笛卡尔坐标系-网格离散空间
/// </summary>
public class IterationPatterns2D 
{
    public static Queue<Vector2Int> auxiliary=>new Queue<Vector2Int>();//之所以不用单例是为了可扩展并行性考虑
    

    /// <summary>
    /// 调用者说明：一般情况这是一个无限循环，需要自己手动设置条件break
    /// </summary>
    /// <param name="origin">起点</param>
    /// <param name="NotAttainCull">不可达点剔除</param>
    /// <param name="GetItem">位置到对象的映射方法</param>
    /// <typeparam name="T">搜索对象类型</typeparam>>
    /// <returns></returns>
    public static IEnumerable<T> BFS<T>(Vector2Int origin,Func<Vector2Int,T> GetItem,Predicate<T> NotAttainCull)
    {
        Queue<Vector2Int> queue = auxiliary;
        queue.Enqueue(origin);
        
        Vector2Int[] extends=new Vector2Int[4];
        while (queue.Count > 0)
        {
            var p = queue.Dequeue();
            var item = GetItem(p);
            yield return item;
            
            //if(isStopCondition(item)) yield break;

            extends[0]=p + Vector2Int.right;
            extends[1]=p + Vector2Int.up;
            extends[2]=p + Vector2Int.left;
            extends[3]=p + Vector2Int.down;
            foreach (var extend in extends)
            {
                if(!NotAttainCull(item))
                    queue.Enqueue(extend);
            }
        }
    }
    public static IEnumerable<T> BFS<T>(Vector2Int origin,Func<Vector2Int,T> GetItem)
    {
        return BFS(origin , GetItem, 
            (t) => { return false; });//均可达
    }
}
