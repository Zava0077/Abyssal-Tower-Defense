using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public Damage damage;
    public Chances chance;
    public Projectile producer;
    private HashSet<GameObject> objectsOnPuddle = new HashSet<GameObject>();
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
        if(other.GetComponent<Entity>()) objectsOnPuddle.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Entity>()) objectsOnPuddle.Remove(other.gameObject);
    }
    IEnumerator Damage()
    {
        while(enabled)
        {
            foreach (var enemy in objectsOnPuddle)
                DoDamage.DealDamage(enemy.GetComponent<Entity>(), null, damage);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
