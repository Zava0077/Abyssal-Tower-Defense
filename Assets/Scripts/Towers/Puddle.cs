using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public Damage damage;
    public Chances chance;
    private HashSet<GameObject> objectsOnPuddle = new HashSet<GameObject>();
    private void OnEnable()
    {
        StartCoroutine(Damage());
        StartCoroutine(DeathSentence());
    }
    IEnumerator DeathSentence()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
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
