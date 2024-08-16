using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] mobs;
    [SerializeField] private float spawnTime = 10f;
    private void OnEnable()
    {
        StartCoroutine(StartSpawning());
    }
    IEnumerator StartSpawning()
    {
        while(enabled)
        {
            yield return new WaitForSeconds(spawnTime);
            GameObject mob = Instantiate(mobs[Random.Range(0, mobs.Length)],transform.position,Quaternion.identity,transform.parent); 
        }
    }
}
