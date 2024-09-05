using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public Damage damage;
    public Chances chance;
    public Projectile producer;
    private HashSet<IDamagable> objectsOnPuddle = new HashSet<IDamagable>();
    private void OnEnable()
    {
        StartCoroutine(Damage());
        StartCoroutine(DeathSentence());
        Entity.onEntityDeath += OnEntityDeath;
    }
    private void OnDisable()
    {
        Entity.onEntityDeath -= OnEntityDeath;
    }
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
        if(otherEntity) objectsOnPuddle.Add(otherEntity);
    }
    private void OnTriggerExit(Collider other)
    {
        Entity otherEntity = other.GetComponent<Entity>();
        if (otherEntity) objectsOnPuddle.Remove(otherEntity);
    }
    IEnumerator Damage()
    {
        while(enabled)
        {
            foreach (var enemy in objectsOnPuddle)
                enemy.GetDamage(damage);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
