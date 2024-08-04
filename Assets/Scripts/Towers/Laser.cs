using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Laser : BulletEffects
{
    Collider collider;
    public override void OnStart(GameObject proj)
    {
        _proj.collidable = false;
        collider = proj.GetComponent<Collider>();
        collider.enabled = false;
        _proj.waitCast = true;
        Player.instance.laser.Play();
    }
    public override void Travel(GameObject proj)
    {
        if (_proj.liveTime > 0.08f)
            collider.enabled = true;
        if (_proj.liveTime > 1.5f)
        {
            Destroy(proj.gameObject);
        }
        proj.transform.position += proj.transform.forward * Vector3.Distance(_proj.position, _proj.target) / 10;
        proj.transform.position = new Vector3(proj.transform.position.x, 1f, proj.transform.position.z);
        proj.transform.rotation.SetEulerRotation(0f, proj.transform.rotation.y, 0f);
    }
    public override void End(GameObject proj)
    {

    }
}
