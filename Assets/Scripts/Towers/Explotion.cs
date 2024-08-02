using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Explotion : MonoBehaviour
{
    float time = 0f;
    public Damage damage;
    private void Update()
    {
        time += Time.deltaTime;
        if (time > 1f)
        {
            time = 0f;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(damage != null && other.GetComponent<Entity>())
        {
            DoDamage.DealDamage(other.GetComponent<Entity>(), null, damage);
        }
    }
}
