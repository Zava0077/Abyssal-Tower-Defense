using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Puddle : MonoBehaviour, IMeshHolder, ITeam
{
    
    public MeshHolder MeshHolder { get; set; }
    public Damage damage;
    public Chances chance;
    [SerializeField] public Mesh mesh;
    public Projectile Producer { get; set; }
    public int TeamId { get; set; }

    private readonly HashSet<IDamagable> objectsOnPuddle = new HashSet<IDamagable>(); 
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
    private IEnumerator DeathSentence()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    private void OnEntityDeath(Entity sender) //или IDamagable
        => objectsOnPuddle.Remove(sender);
    
    private void OnTriggerEnter(Collider other)
    {
        Entity otherEntity = other.GetComponent<Entity>();
        if(otherEntity && TeamId != otherEntity.TeamId) objectsOnPuddle.Add(otherEntity);
    }
    private void OnTriggerExit(Collider other)
    {
        Entity otherEntity = other.GetComponent<Entity>();
        if (otherEntity) 
            objectsOnPuddle.Remove(otherEntity);
    }
    private IEnumerator Damage()
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
