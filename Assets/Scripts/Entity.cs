using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Entity : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public Resistances resistances;
    public Transform transform;
    public List<Status> statuses = new List<Status>();
    public List<float> _damage = new List<float>();
    public List<float> _resist = new List<float>();
    public Damage damage;
    public float attackSpeed;
    public float projSpeed;
    public float multiplierTakeDamage;
    public float agroRadius;
    public Entity _entity;
    public Entity()
    {
        _entity = this;
    }

    public void Awake()
    {
        damage = new Damage(_damage[0], _damage[1], _damage[2], _damage[3], _damage[4]);
        resistances = new Resistances(_resist[0], _resist[1], _resist[2], _resist[3], _resist[4]);
    }
    public void Death()
    {
        Destroy(this.gameObject);
    }
    public void Update()
    {
        foreach(Status status in statuses)
        {
            status.DoStatus();
        }
        if (health < 0)
            Death();
    }
}