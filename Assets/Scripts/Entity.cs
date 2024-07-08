using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Entity : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public Damage damage;
    public Resistances resistances;
    public Transform transform;
    public List<Status> statuses = new List<Status>();
    public float attackSpeed;
    public float multiplierTakeDamage;

    public void Death()
    {
        Destroy(this);
    }

    public void Update()
    {
        foreach(Status status in statuses)
        {
            status.DoStatus();
        }
    }
}
