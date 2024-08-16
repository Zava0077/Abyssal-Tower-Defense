using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterPocket : BulletEffects
{
    GameObject pudd;
       
    public override void OnStart(GameObject proj)
    {
    }
    public override void Travel(GameObject proj)
    {

    }
    public override void End(GameObject proj)
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(0, 99);
        
        if (testrnd < _proj.chance.puddle)
        {
            float size = 0;
            float[] colors = new float[3];
            colors[0] = (_proj.damage._fire + _proj.damage._physical < 255 ? _proj.damage._fire + _proj.damage._physical : 255) / 255;
            colors[1] = (_proj.damage._lightning + _proj.damage._void < 255 ? _proj.damage._lightning + _proj.damage._void : 255) / 255;
            colors[2] = (_proj.damage._cold < 255 ? _proj.damage._cold : 255) / 255;
            for (int i = 0; i < colors.Length; i++)
                if (colors[i] == colors.Max())
                    colors[i] = 1;
                else colors[i] = colors[i] / colors.Max();
            Vector3 from = proj.transform.position;
            foreach (var element in proj.GetComponentsInChildren<Transform>())
                if (element.gameObject.tag == "Projectile")
                    from = element.position;
            foreach (var damage in _proj.damage.GetType().GetFields())//
                size += (float)damage.GetValue(_proj.damage) / 4;
            GameObject[] ground = GameObject.FindGameObjectsWithTag("Ground");
            Vector3 puddPosition = new Vector3(from.x, ground[0].transform.position.y, from.z);
            if (Player.puddles.Count > 0) pudd = Player.puddles.Find(s => !s.activeSelf);
            if (!pudd )
            {
                if(Player.puddles.Count < 128)
                {
                    pudd = Instantiate(Camera.main.GetComponent<Player>().puddle, from, Quaternion.identity, proj.transform.parent);
                    Player.puddles.Add(pudd);
                }
                else pudd = Player.puddles[Player.puddles.Count - 1].gameObject;
            }
            pudd.SetActive(true);
            pudd.transform.position = puddPosition;
            pudd.GetComponent<Puddle>().damage = _proj.damage;
            pudd.GetComponent<Renderer>().material.color = new Color(colors[0], colors[1], colors[2], 0.6f);
            pudd.transform.localScale = new Vector3(size, pudd.transform.localScale.y, size);
            pudd.GetComponent<Puddle>().producer = proj.GetComponent<Projectile>();
            if (Player.instance.pudd.isPlaying) Player.instance.pudd.Stop();

            Player.instance.pudd.Play();
        }
    }
}
