using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Projectile(Damage damage, Entity target)
    {
        this.damage = damage;
        this.target = target;
    }
    public Entity target;
    public Damage damage;
    public float projSpeed;
    public List<BulletEffects> statuses = new List<BulletEffects>();//то, что происходит во время полёта и в конце
    private void Update()
    {
        /*
        foreach (BulletEffects effect in statuses)
        {
            effect.Travel();//дополнительные эффекты снаряда во время полёта,например, за ним остаётся ядовитое облако
        }
        */
        transform.position += transform.right * projSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() == target)
            DoDamage.DealDamage(target, null, damage);
        /*
        foreach (BulletEffects effect in statuses)
        {
            effect.End(); //дополнительные эффекты снаряда в конце полёта, например, взрыв.
        }
        */
        Destroy(this.gameObject);
    }
}
