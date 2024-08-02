using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    float time = 0f;
    float damageTicks = 0f;
    public Damage damage;
    public Chances chance;
    private void Update()
    {
        time += Time.deltaTime;
        damageTicks += Time.deltaTime;
        if (time > 2f)//сделать длительность зависимой
        {
            time = 0f;
            gameObject.SetActive(false);
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if(damageTicks > 2/10)
    //    {
    //        if (damage != null && other.GetComponent<Entity>() != null)
    //            DoDamage.DealDamage(other.GetComponent<Entity>(), null, damage);
    //        damageTicks = 0;
    //    }
    //}
    private void OnCollisionStay(Collision collision)
    {
        if (damageTicks > 2 / 10)
        {
            if (damage != null && collision.gameObject.GetComponent<Entity>())
            {
                DoDamage.DealDamage(collision.gameObject.GetComponent<Entity>(), null, damage);
                damageTicks = 0;
            }
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(damage != null && collision.gameObject.GetComponent<Entity>())
    //        StartCoroutine(Damage(collision));
    //}
    //IEnumerator Damage(Collision collision)
    //{
    //    while (collision.gameObject && gameObject)
    //    {
    //        DoDamage.DealDamage(collision.gameObject.GetComponent<Entity>(), null, damage);
    //        yield return new WaitForSeconds(0.2f);
    //    }
    //}
}
