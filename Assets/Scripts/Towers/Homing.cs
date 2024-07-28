using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Homing : BulletEffects
{
    private Entity enemy;
    public override IEnumerator OnStart(GameObject proj)
    {
        enemy = Tower.twr.FindEnemy(proj, proj.GetComponent<Projectile>().agroRadius, proj.GetComponent<Projectile>().prevEnemy);
        proj.GetComponent<Projectile>().collidable = false;
        proj.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj.transform.right, enemy.transform.position - proj.transform.position, 3.14f, 0f));
        yield return null;
    }
    public override IEnumerator Travel(GameObject proj)
    {
        if (!enemy)
            enemy = Tower.twr.FindEnemy(proj, proj.GetComponent<Projectile>().agroRadius, proj.GetComponent<Projectile>().prevEnemy);
        if (proj.GetComponent<Projectile>().liveTime > (2.5f / proj.GetComponent<Projectile>().projSpeed) * 35f)
            Destroy(proj);
        if (proj.GetComponent<Projectile>().liveTime > (0.15f / proj.GetComponent<Projectile>().projSpeed) * 35f && enemy)
        {
            proj.transform.rotation = Quaternion.Lerp(proj.transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(proj.transform.right, enemy.transform.position - proj.transform.position, 3.14f, 0f)), (0.05f * 35f) / proj.GetComponent<Projectile>().projSpeed);
            proj.GetComponent<Projectile>().collidable = true;
        }
        proj.transform.position += proj.transform.forward * proj.GetComponent<Projectile>().projSpeed * Time.deltaTime;
        proj.transform.position = new Vector3(proj.transform.position.x, 1f, proj.transform.position.z);
        GameObject shadow = Instantiate(Camera.main.GetComponent<Player>().particleShadow, proj.transform.position, proj.transform.rotation, proj.transform.parent);
        shadow.GetComponentInChildren<Fading>().liveTime = 0.05f;
        shadow.GetComponentInChildren<Fading>().color = new Color(125, 0, 125, 255);
        yield return null;
    }
}
