using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : BulletEffects
{
    public override void OnStart(GameObject proj)
    {

    }
    public override void Travel(GameObject proj)
    {
        proj.GetComponent<Projectile>().testTimer += Time.deltaTime;
        if (proj.GetComponent<Projectile>().liveTime > 5f)
            Destroy(proj.gameObject);
        if (proj.GetComponent<Projectile>().liveTime > proj.GetComponent<Projectile>().timeNeed)
            proj.GetComponent<Projectile>().projHeight = -50;
        else
        if (proj.GetComponent<Projectile>().liveTime > proj.GetComponent<Projectile>().timeNeed / 2 && proj.GetComponent<Projectile>().projHeight > 0)
            proj.GetComponent<Projectile>().projHeight *= -1;
        proj.GetComponent<Projectile>().transform.position += proj.transform.forward * proj.GetComponent<Projectile>().projSpeed * Time.deltaTime;
        proj.GetComponent<Projectile>().transform.position += new Vector3(0, proj.GetComponent<Projectile>().projHeight * Time.deltaTime, 0);
        
    }
    public override void End(GameObject proj)
    {

    }
}
