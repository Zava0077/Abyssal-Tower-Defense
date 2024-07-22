using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Bomb : BulletEffects
{
    GameObject expl;
    public bool little;
    public override void Travel(GameObject proj)
    {
        
    }

    public override void End(GameObject proj)
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(0, 99);
        if (proj.GetComponent<Projectile>().owner && testrnd < proj.GetComponent<Projectile>().chance.splash)//
        {
            float size = 0;
            foreach (var damage in proj.GetComponent<Projectile>().damage.GetType().GetFields())//
                size += (float)damage.GetValue(proj.GetComponent<Projectile>().damage) / (!little ? 10 : 7);
            expl = Instantiate(Camera.main.GetComponent<Player>().explotion, proj.transform.position, Quaternion.identity, proj.transform.parent);
            expl.GetComponent<Explotion>().damage = new Damage(0f, 0f, 0f, 15f, !little ? 25f : 50f);
            expl.transform.localScale = new Vector3(expl.transform.localScale.x + size, expl.transform.localScale.y + size, expl.transform.localScale.z + size);
        }
    }
}
