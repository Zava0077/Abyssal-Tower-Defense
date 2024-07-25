using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bounce : BulletEffects
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
        if (testrnd < proj.GetComponent<Projectile>().chance.bounce)//заменить на шанс от башни
        {
            Vector3 from = proj.transform.position;
            foreach (var element in proj.GetComponentsInChildren<Transform>())
                if (element.gameObject.tag == "Projectile")
                    from = element.position;
            Entity nextEnemy = Tower.twr.FindEnemy(proj, proj.GetComponent<Projectile>().agroRadius, proj.GetComponent<Projectile>().prevEnemy);//иногда баунс всё равно может считать противником самого себя
            Vector3 nextTarget = nextEnemy ? nextEnemy.GetComponent<Transform>().position : Vector3.zero;
            if (nextEnemy == null || (proj.GetComponent<Projectile>().prevEnemy != null && proj.GetComponent<Projectile>().prevEnemy.Count > 0 && nextTarget == proj.GetComponent<Projectile>().prevEnemy[0].gameObject.transform.position)) //
            {
                return;
            }
            Tower.twr.Shoot(from, nextTarget, proj.GetComponent<Projectile>().damage, proj, proj.GetComponent<Projectile>().agroRadius, proj.GetComponent<Projectile>().agroRadius, proj.GetComponent<Projectile>().chance,
                proj.GetComponent<Projectile>().effects, proj.GetComponent<Projectile>().projSpeed, proj.transform, proj.GetComponent<Projectile>().prevEnemy);
        }
    }
}
