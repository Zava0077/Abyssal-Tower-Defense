using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public Damage damage;
    public Chances chance;
    bool doDamage = true;
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
    private void OnTriggerStay(Collider other)
    {
        if (!doDamage) return;
        if (damage != null && other.gameObject.GetComponent<Entity>())
        {
            DoDamage.DealDamage(other.gameObject.GetComponent<Entity>(), null, damage);
        }
    }
    IEnumerator Damage()
    {
        doDamage = false;
        yield return new WaitForSeconds(0.2f);
        doDamage = true;
    }
}
