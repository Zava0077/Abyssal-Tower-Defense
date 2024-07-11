using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fraction : BulletEffects
{
    public override void Travel()
    {

    }
    public override void End()
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(1, 99);
        if (base.projectile /*&& base.projectile.GetComponent<Projectile>().prevEnemy*/)
            if (testrnd < 100)//заменить на шанс от башни
            {
                for(int i = 0; i < 2; i++)
                {
                    Vector3 newPosition = new Vector3(base.projectile.transform.position.x, base.projectile.transform.position.y + 2f, base.projectile.transform.position.z);
                    GameObject mini = Instantiate(base.projectile, newPosition, Quaternion.identity, base.projectile.transform.parent);
                    Vector3 nextTarget = new Vector3(projectile.transform.position.x + random.Next(-9, 9), projectile.transform.position.y, projectile.transform.position.z + random.Next(-9, 9));
                    Quaternion rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (nextTarget - newPosition), 3.14f, 0));
                    rotation.SetEulerAngles((3.14f / 180) * rotation.eulerAngles.x, (3.14f / 180) * rotation.eulerAngles.y - (3.14f / 180) * 90, (3.14f / 180) * rotation.eulerAngles.z);
                    mini.transform.rotation = rotation;
                    mini.GetComponent<Projectile>().target = nextTarget;
                    mini.GetComponent<Projectile>().damage = new Damage(base.projectile.GetComponent<Projectile>().damage._fire / 2, base.projectile.GetComponent<Projectile>().damage._cold / 2,
                        base.projectile.GetComponent<Projectile>().damage._lightning / 2, base.projectile.GetComponent<Projectile>().damage._void / 2, base.projectile.GetComponent<Projectile>().damage._physical / 2);
                    mini.GetComponent<Projectile>().owner = base.projectile.GetComponent<Projectile>().owner;
                    mini.GetComponent<Projectile>().agroRadius = base.projectile.GetComponent<Projectile>().agroRadius;
                    mini.GetComponent<Projectile>().archMultiplier = 3;
                    mini.GetComponent<Projectile>().projSpeed = base.projectile.GetComponent<Projectile>().projSpeed;
                    if (mini.GetComponent<Bomb>())
                        mini.GetComponent<Bomb>().little = true;
                    if (projectile.GetComponent<Projectile>())
                        mini.GetComponent<Projectile>().effects = projectile.GetComponent<Projectile>().effects;
                    foreach (var _effect in mini.GetComponent<Projectile>().effects)
                        _effect.projectile = mini;
                }
            }            
    }
}
