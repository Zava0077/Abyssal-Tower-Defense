using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Homing : BulletEffects
{
    private Entity enemy;
    public override void OnStart(GameObject proj)
    {
        enemy = Tower.twr.FindEnemy(proj, proj.GetComponent<Projectile>().agroRadius, proj.GetComponent<Projectile>().prevEnemy);
        proj.GetComponent<Projectile>().collidable = false;
        proj.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj.transform.right, enemy.transform.position - proj.transform.position, 3.14f, 0f));
    }
    public override void Travel(GameObject proj)
    {
        if (!enemy)
            enemy = Tower.twr.FindEnemy(proj, proj.GetComponent<Projectile>().agroRadius, new List<Mob>());
        if (proj.GetComponent<Projectile>().liveTime > proj.GetComponent<Projectile>().timeNeed/4)
            proj.GetComponent<Projectile>().collidable = true;
        if (proj.GetComponent<Projectile>().liveTime > (2.5f / 35f) * proj.GetComponent<Projectile>().projSpeed)
            Destroy(proj.gameObject);
        if (proj.GetComponent<Projectile>().liveTime > (0.15f / 35f)* proj.GetComponent<Projectile>().projSpeed && enemy)
            proj.transform.rotation = Quaternion.Lerp(proj.transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(proj.transform.right, enemy.transform.position - proj.transform.position, 3.14f, 0f)),0.05f);
        proj.transform.position += proj.transform.forward * proj.GetComponent<Projectile>().projSpeed * Time.deltaTime;
        GameObject shadow = Instantiate(Camera.main.GetComponent<Player>().particleShadow, proj.transform.position, proj.transform.rotation, proj.transform.parent);
        shadow.GetComponentInChildren<Fading>().liveTime = 0.1f;
        shadow.GetComponentInChildren<Fading>().color = new Color(125, 0, 125, 255);
    }
    public override void End(GameObject proj)
    {

    }
}
