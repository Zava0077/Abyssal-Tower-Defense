using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : BulletEffects
{
    GameObject expl;
    public override void Travel()
    {
        
    }

    public override void End()
    {
        float size = 0;
        foreach (var damage in GetComponent<Projectile>().owner.damage.GetType().GetFields())
            size += (float)damage.GetValue(GetComponent<Projectile>().owner.damage)/7;
        expl = Instantiate(Camera.main.GetComponent<Player>().explotion, gameObject.transform.position, Quaternion.identity, gameObject.transform.parent);
        expl.GetComponent<Explotion>().damage = new Damage(0f, 0f, 0f, 15f, 50f);
        expl.transform.localScale = new Vector3(expl.transform.localScale.x + size, expl.transform.localScale.y + size, expl.transform.localScale.z + size);
    }
}
