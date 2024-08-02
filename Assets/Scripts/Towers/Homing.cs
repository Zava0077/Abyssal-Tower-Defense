using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Homing : BulletEffects
{
    private Entity enemy;
    float shadCd = 0f;
    public override void OnStart(GameObject proj)
    {
        enemy = Tower.twr.FindEnemy(proj, proj.GetComponent<Projectile>().agroRadius, new Dictionary<float, Entity>(), proj.GetComponent<Projectile>().prevEnemy);
        proj.GetComponent<Projectile>().collidable = false; 
        proj.GetComponent<Projectile>().waitCast = true;
    }
    public override void Travel(GameObject proj)
    {
        shadCd += Time.deltaTime;
        if (!enemy)
            enemy = Tower.twr.FindEnemy(proj, proj.GetComponent<Projectile>().agroRadius, new Dictionary<float, Entity>(), proj.GetComponent<Projectile>().prevEnemy);
        if (proj.GetComponent<Projectile>().liveTime > (2.5f / proj.GetComponent<Projectile>().projSpeed) * 35f)
            Destroy(proj);
        if (proj.GetComponent<Projectile>().liveTime > (0.15f / proj.GetComponent<Projectile>().projSpeed) * 35f && enemy)
        {
            proj.transform.rotation = Quaternion.Lerp(proj.transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(proj.transform.forward, enemy.transform.position - proj.transform.position, 3.14f, 0f)), (0.02f * 35f) / proj.GetComponent<Projectile>().projSpeed);
            proj.GetComponent<Projectile>().collidable = true;
        }
        proj.transform.position += proj.transform.forward * proj.GetComponent<Projectile>().projSpeed * Time.deltaTime;
        proj.transform.position = new Vector3(proj.transform.position.x, 1f, proj.transform.position.z);
    }
    public override void End(GameObject proj)
    {

    }
}
