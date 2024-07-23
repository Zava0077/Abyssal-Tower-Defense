using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBeam : BulletEffects
{
    private Vector3 positionMemory;
    public override void OnStart(GameObject proj)
    {
        positionMemory = proj.transform.position;
    }
    public override void Travel(GameObject proj)
    {
        var element = proj.GetComponentInChildren<Transform>();
        element.localScale = new Vector3(element.localScale.x, element.localScale.y, Vector3.Distance(positionMemory, proj.GetComponent<Projectile>().targetMemory));
        element.position = Vector3.MoveTowards(positionMemory, proj.GetComponent<Projectile>().targetMemory, Vector3.Distance(positionMemory, proj.GetComponent<Projectile>().targetMemory) / 2);
        element.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.right, proj.GetComponent<Projectile>().targetMemory - proj.transform.position, 3.14f, 0));
        if (proj.GetComponent<Projectile>().liveTime > 0.1f)
        {
            foreach (var effect in proj.GetComponent<Projectile>().effects)
                effect.End(proj);
            Destroy(proj);
        }
    }

    public override void End(GameObject proj)
    {
        //поиск врага в диапазоне раз в 10 меньше оригинального => клонирование проджектайла с конца либо ничего
    }
}
