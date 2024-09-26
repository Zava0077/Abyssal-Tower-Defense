using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Puddle : MonoBehaviour, IMeshHolder
{
    public MeshHolder MeshHolder { get; set; }
    public Damage damage;
    public Chances chance;
    [SerializeField] public Mesh mesh;
    public Projectile producer { get; set; }
    private HashSet<IDamagable> objectsOnPuddle = new HashSet<IDamagable>();
    private void OnEnable()
    {
        StartCoroutine(Damage());
        StartCoroutine(DeathSentence());
        Entity.onEntityDeath += OnEntityDeath;
    }
    private void OnDisable()
    {
        objectsOnPuddle.Clear();
        Entity.onEntityDeath -= OnEntityDeath;
    }
    IEnumerator DeathSentence()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    void OnEntityDeath(Entity sender) //или IDamagable
        => objectsOnPuddle.Remove(sender);
    
    private void OnTriggerEnter(Collider other)
    {
        Entity otherEntity = other.GetComponent<Entity>();
        if(otherEntity) objectsOnPuddle.Add(otherEntity);
    }
    private void OnTriggerExit(Collider other)
    {
        Entity otherEntity = other.GetComponent<Entity>();
        if (otherEntity) 
            objectsOnPuddle.Remove(otherEntity);
    }
    IEnumerator Damage()
    {
        while(enabled)
        {
            foreach (var enemy in objectsOnPuddle)
                enemy.GetDamage(damage);
            objectsOnPuddle.ToList().ForEach(obj => Debug.Log(obj.ToString()));
            yield return new WaitForSeconds(0.2f);
        }
    }
}
