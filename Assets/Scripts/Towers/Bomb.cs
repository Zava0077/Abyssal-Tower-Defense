using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Bomb : BulletEffects
{
    GameObject expl;
    public bool little;
    public override void OnStart(GameObject proj)
    {

    }
    public override void Travel(GameObject proj)
    {
        
    }

    public override void End(GameObject proj)
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(0, 99);
        Damage damage1 = proj.GetComponent<Projectile>().damage;
        if (testrnd < proj.GetComponent<Projectile>().chance.splash)//
        {
            float size = 0;
            Vector3 from = proj.transform.position;
            foreach (var element in proj.GetComponentsInChildren<Transform>())
                if (element.gameObject.tag == "Projectile")
                    from = element.position;
            foreach (var damage in proj.GetComponent<Projectile>().damage.GetType().GetFields())//
                size += (float)damage.GetValue(proj.GetComponent<Projectile>().damage) / 7;
            expl = Instantiate(Camera.main.GetComponent<Player>().explotion, from, Quaternion.identity, proj.transform.parent);
            expl.GetComponent<Explotion>().damage = new Damage(damage1._fire, damage1._cold, damage1._lightning, damage1._void + 15f, damage1._physical + 50f);
            expl.transform.localScale = new Vector3(expl.transform.localScale.x + size, expl.transform.localScale.y + size, expl.transform.localScale.z + size);
        }
    }
}
