using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Bomb : BulletEffects
{
    GameObject expl;
    public bool little;
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
        if (testrnd < proj.GetComponent<Projectile>().chance.splash)//
        {
            float size = 0;
            Vector3 from = proj.transform.position;
            foreach (var element in proj.GetComponentsInChildren<Transform>())
                if (element.gameObject.tag == "Projectile")
                    from = element.position;
            foreach (var damage in proj.GetComponent<Projectile>().damage.GetType().GetFields())//
                size += (float)damage.GetValue(proj.GetComponent<Projectile>().damage) / 7;
            if(Player.explotions.Count > 0)
                expl = Player.explotions.Find(s => !s.activeSelf);
            if (!expl)
            {
                if(Player.explotions.Count < 128)
                {
                    expl = Instantiate(Camera.main.GetComponent<Player>().explotion, from, Quaternion.identity, proj.transform.parent);
                    Player.explotions.Add(expl);
                }
                else expl = Player.explotions[Player.explotions.Count - 1];
            }
            expl.SetActive(true);
            expl.transform.position = from;
            expl.GetComponent<Explotion>().damage = new Damage(15f, 0f, 0f, 0f, 50f);
            expl.transform.localScale = new Vector3(5f + size, 5f + size, 5f + size);
        }
    }
}
