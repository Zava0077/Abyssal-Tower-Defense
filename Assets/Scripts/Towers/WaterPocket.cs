using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        float testrnd = random.Next(0, 99);
        float[] colors = new float[3];
        colors[0] = (proj.GetComponent<Projectile>().damage._fire + proj.GetComponent<Projectile>().damage._physical < 255 ? proj.GetComponent<Projectile>().damage._fire + proj.GetComponent<Projectile>().damage._physical : 255)/255;
        colors[1] = (proj.GetComponent<Projectile>().damage._lightning + proj.GetComponent<Projectile>().damage._void < 255 ? proj.GetComponent<Projectile>().damage._lightning + proj.GetComponent<Projectile>().damage._void : 255)/255;
        colors[2] = (proj.GetComponent<Projectile>().damage._cold < 255 ? proj.GetComponent<Projectile>().damage._cold : 255)/255;
        for (int i = 0; i < colors.Length; i++)
            if (colors[i] == colors.Max())
                colors[i] = 1;
            else colors[i] = colors[i] / colors.Max();
        if (proj.GetComponent<Projectile>().owner && testrnd < proj.GetComponent<Projectile>().chance.puddle)
        {
            float size = 0;
            foreach (var damage in proj.GetComponent<Projectile>().damage.GetType().GetFields())//
                size += (float)damage.GetValue(proj.GetComponent<Projectile>().damage) / 4;
            GameObject[] ground = GameObject.FindGameObjectsWithTag("Ground");
            Vector3 puddPosition = new Vector3(proj.transform.position.x, ground[0].transform.position.y, proj.transform.position.z);
            pudd = Instantiate(Camera.main.GetComponent<Player>().puddle, puddPosition, Quaternion.identity, proj.transform.parent);
            pudd.GetComponent<Puddle>().damage = proj.GetComponent<Projectile>().damage;
            pudd.GetComponent<Renderer>().material.color = new Color(colors[0], colors[1], colors[2]);
            pudd.transform.localScale = new Vector3(pudd.transform.localScale.x + size, pudd.transform.localScale.y, pudd.transform.localScale.z + size);

        }
    }
}
