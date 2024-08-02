using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class Status : MonoBehaviour
{
    Status(StatusType type, float Time, Damage damage, Entity entity)
    {
        this.type = type;
        this.time = Time;
        this.entity = entity;
        status = this;
        Type damage1 = typeof(Damage);
        foreach(var num in damage1.GetFields())
        {
            this.damage += (float)num.GetValue(damage);
        }
        this.strength = this.damage / entity.health;
        if(type == StatusType.chill) 
        {
            attackspeed = entity.attackSpeed;
        }
        else if(type == StatusType.shock)
        {
            multiplierTakeDamage = entity.multiplierTakeDamage; 
        }
    }
    public static Status status;
    private float time;
    private float strength;
    private float damage;
    private Entity entity;
    private float attackspeed;
    private float multiplierTakeDamage;
    public enum StatusType
    {
        fire,
        freeze,
        chill,
        shock
    }
    public StatusType type;

    public void DoStatus()
    {
        switch (type)
        {
            case StatusType.fire:
                {
                    entity.health -= damage / 5;
                    break;
                }
            case StatusType.freeze:
                {
                    entity.attackSpeed = 0;
                    break;
                }
            case StatusType.chill:
                {
                    entity.attackSpeed = entity.attackSpeed * (1 - strength);
                    break;
                }
            case StatusType.shock:
                {
                    entity.multiplierTakeDamage = entity.multiplierTakeDamage * (1 - strength);
                    break;
                }
        }
        time -= Time.deltaTime;
        if(time <= 0)
        {
            if(this.type == StatusType.chill || this.type == StatusType.freeze)
            {
                entity.attackSpeed = attackspeed;
            }
            entity.statuses.Remove(this);
        }
    }
}
