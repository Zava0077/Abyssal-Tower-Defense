using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Laser : BulletEffects
{
    public override void Travel(GameObject proj)
    {
        proj.GetComponent<Projectile>().testTimer += Time.deltaTime;
        if (proj.GetComponent<Projectile>().liveTime > 1.5f)
            Destroy(proj.gameObject);
        proj.GetComponent<Projectile>().transform.position += proj.transform.right * Vector3.Distance(proj.GetComponent<Projectile>().position, proj.GetComponent<Projectile>().target) / 6;
        GameObject shadow = Instantiate(Camera.main.GetComponent<Player>().particleShadow, proj.transform.position, proj.transform.rotation, proj.transform.parent);
        shadow.GetComponentInChildren<Fading>().liveTime = 0.1f;
        shadow.GetComponentInChildren<Fading>().color = new Color(255, 0, 0, 255);
    }
    public override void End(GameObject proj)
    {

    }
}
