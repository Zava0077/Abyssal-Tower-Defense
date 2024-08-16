using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Homing : BulletEffects
{
    private Entity enemy;
    Collider collider;
    public override void OnStart(GameObject proj)
    {
        enemy = Tower.twr.FindEnemy(proj, _proj.agroRadius, new Dictionary<float, Entity>(), _proj.prevEnemy);
        collider = proj.GetComponent<Collider>();
        collider.enabled = false;
        _proj.collidable = false; 
        _proj.waitCast = true;
        if (Player.instance.laser.isPlaying)
            Player.instance.laser.Stop();
        //if (Player.instance.laser.isPlaying)
        //    Player.instance.homing.Stop();
        Player.instance.laser.Play();
        Player.instance.homing.Play();
    }
    public override void Travel(GameObject proj)
    {
        if (!enemy)
            enemy = Tower.twr.FindEnemy(proj, _proj.agroRadius, new Dictionary<float, Entity>(), _proj.prevEnemy);
        if (_proj.liveTime > (2.5f / _proj.projSpeed) * 35f)
            Destroy(proj);
        if (_proj.liveTime > (0.15f / _proj.projSpeed) * 35f && enemy)
        {
            proj.transform.rotation = Quaternion.Lerp(proj.transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(proj.transform.forward, enemy.transform.position - proj.transform.position, 3.14f, 0f)), (0.02f * 35f) / _proj.projSpeed);
            collider.enabled = true;
        }
        proj.transform.position += proj.transform.forward * _proj.projSpeed * Time.deltaTime;
        proj.transform.position = new Vector3(proj.transform.position.x, 1f, proj.transform.position.z);
    }
    public override void End(GameObject proj)
    {
        Player.instance.homing.Stop();
    }
}
