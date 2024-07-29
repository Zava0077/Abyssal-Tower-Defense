using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Fraction : BulletEffects
{
    public override void End(GameObject proj)
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(0, 99);
        if (testrnd < proj.GetComponent<Projectile>().chance.shatter)//заменить на шанс от башни
        {
            for (int i = 0; i < 2; i++)
            {
                bool _elec = false;
                int modifier = 1;
                Vector3 from = proj.transform.position;
                foreach (var element in proj.GetComponentsInChildren<Transform>())
                    if (element.gameObject.tag == "Projectile")
                        from = element.position;
                foreach (var _effect in proj.GetComponent<Projectile>().effects)
                    if (_effect.ToString().Contains("(ElectricBeam)"))
                    {
                        _elec = true;
                        modifier = 2;
                    }
                  
                Vector3 nextTarget = new Vector3(from.x + random.Next(-9 * modifier, 9 * modifier), from.y, from.z + random.Next(-9 * modifier, 9 * modifier));
                Chances newChance = proj.GetComponent<Projectile>().chance;
                //newChance.shatter /= 2;
                Tower.twr.Shoot(from, nextTarget, new Damage(proj.GetComponent<Projectile>().damage._fire / 2, proj.GetComponent<Projectile>().damage._cold / 2,
                    proj.GetComponent<Projectile>().damage._lightning / 2, proj.GetComponent<Projectile>().damage._void / 2, proj.GetComponent<Projectile>().damage._physical / 2), proj, proj.GetComponent<Projectile>().agroRadius, 3f,
                    newChance, proj.GetComponent<Projectile>().effects, proj.GetComponent<Projectile>().projSpeed, proj.transform, !_elec ? null : proj.GetComponent<Projectile>().prevEnemy,
                    new Vector3(proj.transform.localScale.x / 1.5f, proj.transform.localScale.y / 1.5f, proj.transform.localScale.z / 1.5f));
            }
        }
    }
}
