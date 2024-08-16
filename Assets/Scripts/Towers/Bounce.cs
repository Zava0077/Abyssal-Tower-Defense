using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bounce : BulletEffects
{
    Projectile _proj;
    public override void OnStart(GameObject proj)
    {
        _proj = proj.GetComponent<Projectile>();
    }
    public override void Travel(GameObject proj)
    {

    }
    public override void End(GameObject proj)
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(0, 99);
        if (testrnd < _proj.chance.bounce)//заменить на шанс от башни
        {
            Vector3 from = proj.transform.position;
            foreach (var element in proj.GetComponentsInChildren<Transform>())
                if (element.gameObject.tag == "Projectile")
                    from = element.position;
            Entity nextEnemy = Tower.twr.FindEnemy(proj, _proj.agroRadius, new Dictionary<float, Entity>(), _proj.prevEnemy);//иногда баунс всё равно может считать противником самого себя
            Vector3 nextTarget = nextEnemy ? nextEnemy.GetComponent<Transform>().position : Vector3.zero;
            if (nextEnemy == null || (_proj.prevEnemy != null && _proj.prevEnemy.Count > 0 && nextTarget == _proj.prevEnemy[0].gameObject.transform.position)) //
            {
                return;
            }
            Tower.twr.Shoot(from, nextTarget, _proj.damage, proj, _proj.agroRadius, _proj.agroRadius, _proj.chance,
                _proj.effects, _proj.projSpeed, proj.transform, _proj.prevEnemy);
            if (Player.instance.bounce.isPlaying) Player.instance.bounce.Stop();

            Player.instance.bounce.Play();
        }
    }
}
