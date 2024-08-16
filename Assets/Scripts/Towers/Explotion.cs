using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Explotion : MonoBehaviour
{
    public Projectile producer;
    public Damage damage;
    private void OnEnable()
    {
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
        if(damage != null && other.GetComponent<Entity>())
        {
            DoDamage.DealDamage(other.GetComponent<Entity>(), null, damage);
        }
    }
}
