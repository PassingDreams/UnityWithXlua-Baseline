using System.Collections.Generic;
using UnityEngine;


//采取克隆prefab方法产生新对象
public class RandomGameObjectPool: RandomObjectPool<GameObject>
{

    private GameObject sceneHangNode;//transform挂载点
    private List<GameObject> prefabs;
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="nameForPool">缺省即默认以prefab命名</param>
    public RandomGameObjectPool(List<GameObject> prefabs,string nameForPool="")
    {
        if (prefabs==null || prefabs.Count == 0)
        {
            return;
        }
        if (nameForPool.Equals(""))
            nameForPool = prefabs[0].name + " random pool";
        sceneHangNode =new GameObject(nameForPool);
        poolName = nameForPool;

        
        InstantiateMethod = () => { return GameObject.Instantiate(prefabs[Random.Range(0,prefabs.Count)]); };

        OnFillpoolProcessor = (go) =>
        {
            go.SetActive(false);
            go.transform.SetParent(sceneHangNode.transform);

        };

        OnEnpoolProcessor = (go) =>
        {
            go.SetActive(false);
            go.transform.SetParent( sceneHangNode.transform);
        };

        OnDepoolProcessor = (go) =>
        {
            go.SetActive(true);
            go.transform.SetParent(null);
        };
    }

}
