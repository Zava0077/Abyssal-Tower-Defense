using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Fraction : BulletEffects
{
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
        if (testrnd < _proj.chance.shatter)//заменить на шанс от башни
        {
            for (int i = 0; i < 2; i++)
            {
                bool _elec = false;
                int modifier = 1;
                Vector3 from = proj.transform.position;
                foreach (var element in proj.GetComponentsInChildren<Transform>())
                    if (element.gameObject.tag == "Projectile")
                        from = element.position;
                foreach (var _effect in _proj.effects)
                {
                    if (_effect.ToString().Contains("(ElectricBeam)"))
                    {
                        _elec = true;
                        modifier = 2;
                    }
                }
                
                Vector3 nextTarget = new Vector3(from.x + random.Next(-9 * modifier, 9 * modifier), from.y, from.z + random.Next(-9 * modifier, 9 * modifier));
                Chances newChance = new Chances(_proj.chance.bounce, _proj.chance.splash, _proj.chance.puddle,
                    _proj.chance.shatter/2f, _proj.chance.doubleAttack, _proj.chance.crit,
                    _proj.chance.status, _proj.chance.pierce);
                Tower.twr.Shoot(from, nextTarget, new Damage(_proj.damage._fire / 2, _proj.damage._cold / 2,
                    _proj.damage._lightning / 2, _proj.damage._void / 2, _proj.damage._physical / 2), proj, _proj.agroRadius, 3f,
                    newChance, _proj.effects, _proj.projSpeed, proj.transform, !_elec ? null : _proj.prevEnemy,
                    new Vector3(proj.transform.localScale.x / 1.5f, proj.transform.localScale.y / 1.5f, proj.transform.localScale.z / 1.5f));
            }
            Player.instance.fraction.Play();
        }
    }
  
}
