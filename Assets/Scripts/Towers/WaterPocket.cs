using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPocket : BulletEffects
{
    GameObject pudd;
    public override void Travel()
    {

    }

    public override void End()
    {
        float size = 0;
        foreach (var damage in GetComponent<Projectile>().owner.damage.GetType().GetFields())
            size += (float)damage.GetValue(GetComponent<Projectile>().owner.damage);
        pudd = Instantiate(Camera.main.GetComponent<Player>().puddle, gameObject.transform.position, Quaternion.identity, gameObject.transform.parent);
        pudd.transform.localScale = new Vector3(pudd.transform.localScale.x + size, pudd.transform.localScale.y, pudd.transform.localScale.z + size);
    }
}
