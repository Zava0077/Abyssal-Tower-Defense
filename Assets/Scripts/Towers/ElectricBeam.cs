using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBeam : BulletEffects
{
    private Vector3 positionMemory;
    public override void OnStart(GameObject proj)
    {
        positionMemory = proj.transform.position;
        proj.GetComponent<Projectile>().collidable = false;
        Damage damage = proj.GetComponent<Projectile>().damage;
        proj.GetComponent<Projectile>().damage = new Damage(damage._fire / 3, damage._cold / 3, damage._lightning / 3, damage._void / 3, damage._physical / 3);
        var element = proj.GetComponentInChildren<Transform>();
        element.localScale = new Vector3(element.localScale.x, element.localScale.y, Vector3.Distance(positionMemory, proj.GetComponent<Projectile>().targetMemory));
        element.position = Vector3.MoveTowards(positionMemory, proj.GetComponent<Projectile>().targetMemory, Vector3.Distance(positionMemory, proj.GetComponent<Projectile>().targetMemory) / 2);
        element.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.right, proj.GetComponent<Projectile>().targetMemory - proj.transform.position, 3.14f, 0));

    }
    public override void Travel(GameObject proj)
    {
        if (proj.GetComponent<Projectile>().liveTime > 0.2f)
        {
            foreach (var effect in proj.GetComponent<Projectile>().effects)
                effect.End(proj);
            Destroy(proj);
        }
    }

    public override void End(GameObject proj)
    {
        Vector3 from = proj.transform.position;
        Damage damage1 = proj.GetComponent<Projectile>().damage;
        foreach (var element in proj.GetComponentsInChildren<Transform>())
            if (element.gameObject.tag == "Projectile")
                from = element.position;
        GameObject expl = Instantiate(Camera.main.GetComponent<Player>().explotion, from, Quaternion.identity, proj.transform.parent);
        expl.GetComponent<Explotion>().damage = new Damage(0f, 0f, damage1._lightning * 3 + damage1._physical * 3 + damage1._fire * 3 + damage1._void * 3 + damage1._cold * 3, 0f, 0f);
        expl.GetComponent<Renderer>().material.color = new Color(0f, 0.35f, 1f);
        expl.transform.localScale = new Vector3(5f, 5f, 5f);
    }
}
