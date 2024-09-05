using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    private List<T> pool = new();
    private List<Mesh> poolMeshes = new();
    private int limit;
    public ObjectPool(int lim) 
    {
        limit = lim;
    }
    public T PullObject(T prefab, Vector3 where, Mesh nMesh, bool activeFromDefault = true)
    {
        T objectFromPool = pool.Find(item => !item.isActiveAndEnabled);
        if (objectFromPool == null)
        {
            if (pool.Count < limit)
            {
                pool.Add(Instantiate(prefab));
                objectFromPool = pool.Last();
                poolMeshes.Add(nMesh);
            }
            else
                objectFromPool = pool.First();
        }
        int objIndex = pool.FindIndex(obj => obj == objectFromPool);
        //Меняем меш, если требуется
        if (nMesh != poolMeshes[objIndex])
            poolMeshes[pool.FindIndex(obj => obj == objectFromPool)] = nMesh;
        objectFromPool.transform.position = where;
        //Убрал окраску, она будет происходить после выполнения PullObject.
        objectFromPool.gameObject.SetActive(activeFromDefault);
        return objectFromPool;
    }
}
