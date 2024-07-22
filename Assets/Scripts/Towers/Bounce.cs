using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bounce : BulletEffects
{
    public override void Travel(GameObject proj)
    {
        
    }
    public override void End(GameObject proj)
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(0, 99);
            if(proj.GetComponent<Projectile>().owner && testrnd < proj.GetComponent<Projectile>().chance.bounce)//заменить на шанс от башни
            {
                Vector3 newPosition = new Vector3(proj.transform.position.x, proj.transform.position.y, proj.transform.position.z);
                GameObject clone = Instantiate(proj, newPosition, Quaternion.identity, proj.transform.parent);
                Entity nextEnemy = Tower.twr.FindEnemy(clone, clone.GetComponent<Projectile>().agroRadius, proj.GetComponent<Projectile>().prevEnemy);
                Vector3 nextTarget = nextEnemy ? nextEnemy.GetComponent<Transform>().position : Vector3.zero;
                if (nextEnemy == null || (proj.GetComponent<Projectile>().prevEnemy != null && nextTarget == proj.GetComponent<Projectile>().prevEnemy.gameObject.transform.position)) //
                {
                    Destroy(clone);
                    return;
                }
                Quaternion rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (nextTarget - newPosition), 3.14f, 0));
                rotation.SetEulerAngles((3.14f / 180) * rotation.eulerAngles.x, (3.14f / 180) * rotation.eulerAngles.y - (3.14f / 180) * 90, (3.14f / 180) * rotation.eulerAngles.z);
                clone.transform.rotation = rotation;
                clone.GetComponent<Projectile>().target = nextTarget;
                clone.GetComponent<Projectile>().damage = proj.GetComponent<Projectile>().damage;
                clone.GetComponent<Projectile>().owner = proj.GetComponent<Projectile>().owner;
                clone.GetComponent<Projectile>().chance = proj.GetComponent<Projectile>().chance;
                clone.GetComponent<Projectile>().agroRadius = proj.GetComponent<Projectile>().agroRadius;
                clone.GetComponent<Projectile>().prevEnemy = proj.GetComponent<Projectile>().prevEnemy;
                if (proj.GetComponent<Projectile>())
                    clone.GetComponent<Projectile>().effects = proj.GetComponent<Projectile>().effects;
                //foreach (var _effect in clone.GetComponent<Projectile>().effects)
                //    _effect.projectile = clone;
            }
    }
}
