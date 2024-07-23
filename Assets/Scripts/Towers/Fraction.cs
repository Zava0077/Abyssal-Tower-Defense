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
        if (testrnd < proj.GetComponent<Projectile>().chance.shatter)//заменить на шанс от башни
        {
            for (int i = 0; i < 2; i++)
            {
                //Vector3 newPosition = new Vector3(proj.transform.position.x, proj.transform.position.y + 2f, proj.transform.position.z);
                //GameObject mini = Instantiate(proj, newPosition, Quaternion.identity, proj.transform.parent);
                //mini.transform.localScale = new Vector3(proj.transform.localScale.x/1.5f, proj.transform.localScale.y/ 1.5f, proj.transform.localScale.z/ 1.5f);
                int modifier = 1;
                Vector3 from = proj.transform.position;
                foreach (var element in proj.GetComponentsInChildren<Transform>())
                    if (element.gameObject.tag == "Projectile")
                        from = element.position;
                foreach (var _effect in proj.GetComponent<Projectile>().effects)
                    if (_effect.ToString().Contains("(ElectricBeam)"))
                        modifier = 2;
                Vector3 nextTarget = new Vector3(from.x + random.Next(-9* modifier, 9 * modifier), from.y, from.z + random.Next(-9 * modifier, 9 * modifier));
                Tower.twr.Shoot(from, nextTarget, new Damage(proj.GetComponent<Projectile>().damage._fire / 2, proj.GetComponent<Projectile>().damage._cold / 2,
                    proj.GetComponent<Projectile>().damage._lightning / 2, proj.GetComponent<Projectile>().damage._void / 2, proj.GetComponent<Projectile>().damage._physical / 2), proj, proj.GetComponent<Projectile>().agroRadius, 3f,
                    proj.GetComponent<Projectile>().chance, proj.GetComponent<Projectile>().effects, proj.GetComponent<Projectile>().projSpeed, proj.transform, proj.GetComponent<Projectile>().prevEnemy,
                    new Vector3(proj.transform.localScale.x / 1.5f, proj.transform.localScale.y / 1.5f, proj.transform.localScale.z / 1.5f));

                //Quaternion rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj.transform.forward, (nextTarget - newPosition), 3.14f, 0));
                //rotation.SetEulerAngles((3.14f / 180) * rotation.eulerAngles.x, (3.14f / 180) * rotation.eulerAngles.y - (3.14f / 180) * 90, (3.14f / 180) * rotation.eulerAngles.z);
                //mini.transform.rotation = rotation;
                //mini.GetComponent<Projectile>().target = nextTarget;
                //mini.GetComponent<Projectile>().damage = new Damage(proj.GetComponent<Projectile>().damage._fire / 2, proj.GetComponent<Projectile>().damage._cold / 2,
                //    proj.GetComponent<Projectile>().damage._lightning / 2, proj.GetComponent<Projectile>().damage._void / 2, proj.GetComponent<Projectile>().damage._physical / 2);
                //mini.GetComponent<Projectile>().owner = proj.GetComponent<Projectile>().owner;
                //mini.GetComponent<Projectile>().chance = proj.GetComponent<Projectile>().chance;
                //mini.GetComponent<Projectile>().agroRadius = proj.GetComponent<Projectile>().agroRadius;
                //mini.GetComponent<Projectile>().archMultiplier = 3;
                //mini.GetComponent<Projectile>().projSpeed = proj.GetComponent<Projectile>().projSpeed;
                //mini.GetComponent<Projectile>().prevEnemy = null;
                //if (mini.GetComponent<Bomb>())
                //    mini.GetComponent<Bomb>().little = true;
                //if (proj.GetComponent<Projectile>())
                //    mini.GetComponent<Projectile>().effects = proj.GetComponent<Projectile>().effects;
                //foreach (var _effect in mini.GetComponent<Projectile>().effects)
                //    _effect.projectile = mini;
            }
        }  
    }
}
