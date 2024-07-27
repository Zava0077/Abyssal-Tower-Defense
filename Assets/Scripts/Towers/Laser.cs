using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Laser : BulletEffects
{
    public override void OnStart(GameObject proj)
    {
        proj.GetComponent<Projectile>().collidable = false;
    }
    public override void Travel(GameObject proj)
    {
        if (proj.GetComponent<Projectile>().liveTime > 0.05f)
            proj.GetComponent<Projectile>().collidable = true;
        if (proj.GetComponent<Projectile>().liveTime > 1.5f)
            Destroy(proj.gameObject);
        proj.transform.position += proj.transform.right * Vector3.Distance(proj.GetComponent<Projectile>().position, proj.GetComponent<Projectile>().target) / 8;
        proj.transform.position = new Vector3(proj.transform.position.x, 1f, proj.transform.position.z);
        GameObject shadow = Instantiate(Camera.main.GetComponent<Player>().particleShadow, proj.transform.position, proj.transform.rotation, proj.transform.parent);
        shadow.GetComponentInChildren<Fading>().liveTime = 0.1f;
        shadow.GetComponentInChildren<Fading>().color = new Color(255, 0, 0, 255);
    }
    public override void End(GameObject proj)
    {

    }
}
