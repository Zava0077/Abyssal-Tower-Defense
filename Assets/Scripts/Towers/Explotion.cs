using System.Collections;
using UnityEngine;

public class Explotion : MonoBehaviour, IMeshHolder
{
    public MeshHolder MeshHolder { get; set; }

    public Projectile producer { get; set; }
    public Damage damage;
    private void OnEnable()
    {
        StartCoroutine(DeathSentence()); 
        Entity.onEntityDeath += OnEntityDeath;
    }
    private void OnDisable() => Entity.onEntityDeath -= OnEntityDeath;
    IEnumerator DeathSentence()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    void OnEntityDeath(object sender)
    {
        if (producer != null && sender.GetType() == typeof(Mob) && producer.prevEnemy != null && producer.prevEnemy.Count > 0)
            producer.prevEnemy.Remove((Mob)sender);
    }
    private void OnTriggerEnter(Collider other)
    {
        Entity otherEntity = other.GetComponent<Entity>();
        if(otherEntity)
        {
            otherEntity.GetDamage(damage);
        }
    }
}
