using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    private List<T> pool = new();
    private List<Mesh> poolMeshes = new();
    public T pulledObj;
    private int limit;
    public ObjectPool(int lim) 
    {
        limit = lim;
    }
    public IEnumerator<T> PullObject(T prefab, Vector3 where, Mesh nMesh, bool activeFromDefault = true, int delay = 0)
    {
        //добавить паузу при наличии            
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
            {
                for (int i = 0; i < delay; i++)
                    new WaitForNextFrameUnit();
                objectFromPool = pool.First();
            }
        }
        int objIndex = pool.FindIndex(obj => obj == objectFromPool);
        if (nMesh != poolMeshes[objIndex])
            poolMeshes[pool.FindIndex(obj => obj == objectFromPool)] = nMesh;
        objectFromPool.transform.position = where;
        objectFromPool.gameObject.SetActive(activeFromDefault);
        pulledObj = objectFromPool;
        yield return objectFromPool;
    }
}
