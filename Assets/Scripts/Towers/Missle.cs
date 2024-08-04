using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : BulletEffects
{
    public override void OnStart(GameObject proj)
    {
        Player.instance.shoot.Play();
    }
    public override void Travel(GameObject proj)
    {
        if (_proj.liveTime > 5f)
            Destroy(proj);
        if (_proj.liveTime > _proj.timeNeed)
            _proj.projHeight = -50;
        else
        if (_proj.liveTime > _proj.timeNeed / 2 && _proj.projHeight > 0)
            _proj.projHeight *= -1;
        _proj.transform.position += proj.transform.forward * _proj.projSpeed * Time.deltaTime;
        _proj.transform.position += new Vector3(0, _proj.projHeight * Time.deltaTime, 0);
    }
    public override void End(GameObject proj)
    {

    }
}
