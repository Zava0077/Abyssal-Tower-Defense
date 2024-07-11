using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bounce : BulletEffects
{
    GameObject clone;
    public override void Travel()
    {
        
    }
    public override void End()
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(1, 99);
       if (base.projectile && base.projectile.GetComponent<Projectile>().prevEnemy)
            if(testrnd < 100)//заменить на шанс от башни
            {
                // Quaternion rotation = new Quaternion(base.projectile.transform.rotation.x, base.projectile.transform.rotation.y, base.projectile.transform.rotation.z, base.projectile.transform.rotation.w);
                Vector3 newPosition = new Vector3(base.projectile.transform.position.x, base.projectile.transform.position.y, base.projectile.transform.position.z);
                clone = Instantiate(base.projectile, newPosition, Quaternion.identity, base.projectile.transform.parent);
                Vector3 nextTarget = Tower.FindEnemy(clone, clone.GetComponent<Projectile>().agroRadius, base.projectile.GetComponent<Projectile>().prevEnemy).GetComponent<Transform>().position;
                if (nextTarget == projectile.GetComponent<Projectile>().prevEnemy.gameObject.transform.position)
                {
                    Destroy(clone);
                    return;
                }
                Quaternion rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (nextTarget - newPosition), 3.14f, 0));
                rotation.SetEulerAngles((3.14f / 180) * rotation.eulerAngles.x, (3.14f / 180) * rotation.eulerAngles.y - (3.14f / 180) * 90, (3.14f / 180) * rotation.eulerAngles.z);
                clone.transform.rotation = rotation;
                clone.GetComponent<Projectile>().target = nextTarget;
                clone.GetComponent<Projectile>().damage = base.projectile.GetComponent<Projectile>().damage;
                clone.GetComponent<Projectile>().owner = base.projectile.GetComponent<Projectile>().owner;
                clone.GetComponent<Projectile>().agroRadius = base.projectile.GetComponent<Projectile>().agroRadius;
                clone.GetComponent<Projectile>().prevEnemy = base.projectile.GetComponent<Projectile>().prevEnemy;
                if (projectile.GetComponent<Projectile>())
                    clone.GetComponent<Projectile>().effects = projectile.GetComponent<Projectile>().effects;
                foreach (var _effect in clone.GetComponent<Projectile>().effects)
                    _effect.projectile = clone;
            }
        
        //if(projectile)
        //{
        //    float agroRadius = projectile.GetComponent<Projectile>().agroRadius;
        //    clone = Instantiate(projectile, projectile.transform.position, Quaternion.identity, projectile.transform.parent);
        //    if (Random.Range(1, 100) < 15)
        //        Tower.Shoot(projectile, Tower.FindEnemy(projectile, agroRadius).GetComponent<Transform>().position, projectile.GetComponent<Projectile>().damage, clone, agroRadius);
        //}
    }
}
