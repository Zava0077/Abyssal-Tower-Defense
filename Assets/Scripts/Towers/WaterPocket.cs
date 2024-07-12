using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPocket : BulletEffects
{
    GameObject pudd;
    public override void Travel(GameObject proj)
    {

    }

    public override void End(GameObject proj)
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(1, 99);
        if(proj.GetComponent<Projectile>().owner && testrnd < proj.GetComponent<Projectile>().owner.GetComponent<Tower>().chance.puddle)
        {
            float size = 0;
            foreach (var damage in proj.GetComponent<Projectile>().damage.GetType().GetFields())//
                size += (float)damage.GetValue(proj.GetComponent<Projectile>().damage) / 4;
            GameObject[] ground = GameObject.FindGameObjectsWithTag("Ground");
            Vector3 puddPosition = new Vector3(proj.transform.position.x, ground[0].transform.position.y, proj.transform.position.z);
            pudd = Instantiate(Camera.main.GetComponent<Player>().puddle, puddPosition, Quaternion.identity, proj.transform.parent);
            pudd.GetComponent<Puddle>().damage = proj.GetComponent<Projectile>().damage;
            pudd.GetComponent<Renderer>().material.SetColor("White",new Color(proj.GetComponent<Projectile>().damage._fire + proj.GetComponent<Projectile>().damage._physical, 0, proj.GetComponent<Projectile>().damage._cold));
            pudd.transform.localScale = new Vector3(pudd.transform.localScale.x + size, pudd.transform.localScale.y, pudd.transform.localScale.z + size);

        }
    }
}
