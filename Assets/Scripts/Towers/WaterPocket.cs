using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPocket : BulletEffects
{
    GameObject pudd;
    public override void Travel(GameObject proj)
    {

    }

    public override void End(GameObject proj)
    {
        float size = 0;
        foreach (var damage in GetComponent<Projectile>().owner.GetComponent<Projectile>().damage.GetType().GetFields())
            size += (float)damage.GetValue(GetComponent<Projectile>().owner.GetComponent<Projectile>().damage);
        pudd = Instantiate(Camera.main.GetComponent<Player>().puddle, proj.transform.position, Quaternion.identity, proj.transform.parent);
        pudd.transform.localScale = new Vector3(pudd.transform.localScale.x + size, pudd.transform.localScale.y, pudd.transform.localScale.z + size);
    }
}
