using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fraction : BulletEffects
{
    public override void Travel(GameObject proj)
    {

    }
    public override void End(GameObject proj)
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(1, 99);
        if (base.projectile)
            if (testrnd < proj.GetComponent<Projectile>().owner.GetComponent<Tower>().chance.shatter)//заменить на шанс от башни
            {
                for(int i = 0; i < 2; i++)
                {
                    Vector3 newPosition = new Vector3(proj.transform.position.x, proj.transform.position.y + 2f, proj.transform.position.z);
                    GameObject mini = Instantiate(proj, newPosition, Quaternion.identity, proj.transform.parent);
                    Vector3 nextTarget = new Vector3(proj.transform.position.x + random.Next(-9, 9), proj.transform.position.y, proj.transform.position.z + random.Next(-9, 9));
                    Quaternion rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (nextTarget - newPosition), 3.14f, 0));
                    rotation.SetEulerAngles((3.14f / 180) * rotation.eulerAngles.x, (3.14f / 180) * rotation.eulerAngles.y - (3.14f / 180) * 90, (3.14f / 180) * rotation.eulerAngles.z);
                    mini.transform.rotation = rotation;
                    mini.GetComponent<Projectile>().target = nextTarget;
                    mini.GetComponent<Projectile>().damage = new Damage(proj.GetComponent<Projectile>().damage._fire / 2, proj.GetComponent<Projectile>().damage._cold / 2,
                        base.projectile.GetComponent<Projectile>().damage._lightning / 2, proj.GetComponent<Projectile>().damage._void / 2, proj.GetComponent<Projectile>().damage._physical / 2);
                    mini.GetComponent<Projectile>().owner = proj.GetComponent<Projectile>().owner;
                    mini.GetComponent<Projectile>().agroRadius = proj.GetComponent<Projectile>().agroRadius;
                    mini.GetComponent<Projectile>().archMultiplier = 3;
                    mini.GetComponent<Projectile>().projSpeed = proj.GetComponent<Projectile>().projSpeed;
                    if (mini.GetComponent<Bomb>())
                        mini.GetComponent<Bomb>().little = true;
                    if (projectile.GetComponent<Projectile>())
                        mini.GetComponent<Projectile>().effects = proj.GetComponent<Projectile>().effects;
                    foreach (var _effect in mini.GetComponent<Projectile>().effects)
                        _effect.projectile = mini;
                }
            }            
    }
}
