using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Laser : BulletEffects
{
    float shadCd = 0f;
    public override void OnStart(GameObject proj)
    {
        proj.GetComponent<Projectile>().collidable = false;
        Entity.projsWShadows.Add(proj);
    }
    public override void Travel(GameObject proj)
    {
        shadCd += Time.deltaTime;
        if (proj.GetComponent<Projectile>().liveTime > 0.08f)
            proj.GetComponent<Projectile>().collidable = true;
        if (proj.GetComponent<Projectile>().liveTime > 1.5f)
        {
            Destroy(proj.gameObject);
            Entity.projsWShadows.Remove(proj);
        }
        proj.transform.position += proj.transform.forward * Vector3.Distance(proj.GetComponent<Projectile>().position, proj.GetComponent<Projectile>().target) / 10;
        proj.transform.position = new Vector3(proj.transform.position.x, 1f, proj.transform.position.z);
        proj.transform.rotation.SetEulerRotation(0f, proj.transform.rotation.y, 0f);
    }
 
}
