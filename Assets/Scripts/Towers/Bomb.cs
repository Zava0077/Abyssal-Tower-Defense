using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Bomb : BulletEffects
{
    GameObject expl;
    public bool little;
    public override void Travel()
    {
        
    }

    public override void End()
    {
        float size = 0;
        if(projectile)
        {
            foreach (var damage in projectile.GetComponent<Projectile>().damage.GetType().GetFields())
                size += (float)damage.GetValue(projectile.GetComponent<Projectile>().damage) / (!little ? 10 : 7);
            expl = Instantiate(Camera.main.GetComponent<Player>().explotion, projectile.transform.position, Quaternion.identity, projectile.transform.parent);
            expl.GetComponent<Explotion>().damage = new Damage(0f, 0f, 0f, 15f, !little ? 25f : 50f);
            expl.transform.localScale = new Vector3(expl.transform.localScale.x + size, expl.transform.localScale.y + size, expl.transform.localScale.z + size);
        }
    }
}
