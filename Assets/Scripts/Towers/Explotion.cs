using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Explotion : MonoBehaviour
{
    public Damage damage;
    private void OnEnable()
    {
        StartCoroutine(DeathSentence());
    }
    IEnumerator DeathSentence()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(damage != null && other.GetComponent<Entity>())
        {
            DoDamage.DealDamage(other.GetComponent<Entity>(), null, damage);
        }
    }
}
