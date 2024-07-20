using UnityEngine;


//采取克隆prefab方法产生新对象
public class GameObjectPool: ObjectPool<GameObject>
{

    private GameObject sceneHangNode;//transform挂载点
    private GameObject prefab;
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="nameForPool">缺省即默认以prefab命名</param>
    public GameObjectPool(GameObject prefab,string nameForPool="")
    {
        if (nameForPool.Equals(""))
            nameForPool = prefab.name + " pool";
        sceneHangNode =new GameObject(nameForPool);
        poolName = nameForPool;

        
        
        
        InstantiateMethod = () => { return GameObject.Instantiate(prefab); };

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
