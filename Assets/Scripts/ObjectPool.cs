using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour, IMeshHolder
{
    private List<T> pool = new();
    private List<MeshHolder> poolMeshes = new();
    public T pulledObj;
    private int limit;
    public int PoolFullness
    {
        get => pool.Count;
    }
    public ObjectPool(int lim) 
    {
        limit = lim;
    }
    public IEnumerator<T> PullObject(T prefab, Vector3 where, Mesh nMesh, bool forcedPull = true, bool activeFromDefault = true, int delay = 0)
    { 
        T objectFromPool = pool.Find(item => !item.isActiveAndEnabled);
        if (objectFromPool == null)
        {
            if (pool.Count < limit)
            {
                pool.Add(Instantiate(prefab));
                objectFromPool = pool.Last();
                //if(nMesh != null)
                //    poolMeshes.Add(prefab.MeshHolder);
            }
            else if (forcedPull)
            {
                for (int i = 0; i < delay; i++)
                    new WaitForNextFrameUnit();
                objectFromPool = pool.First();
            }
        }
        int objIndex = pool.FindIndex(obj => obj == objectFromPool);
        //if (nMesh != null && objIndex > 0 && nMesh != poolMeshes[objIndex].Mesh) //null exception
        //    poolMeshes[objIndex].Mesh = nMesh;
        objectFromPool.transform.position = where;
        objectFromPool.gameObject.SetActive(activeFromDefault);
        pulledObj = objectFromPool;
        yield return objectFromPool;
    }
}
