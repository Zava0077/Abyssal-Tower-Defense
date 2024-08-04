using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cold : BulletEffects
{
    public override void OnStart(GameObject proj)
    {
        Damage damage = _proj.damage;
        _proj.damage = new Damage(damage._fire / 3, damage._cold / 3, damage._lightning / 3, damage._void / 3, damage._physical / 3);
    }
    public override void Travel(GameObject proj)
    {

    }

    public override void End(GameObject proj)
    {
        Vector3 from = proj.transform.position;
        Damage damage1 = _proj.damage;
        foreach (var element in proj.GetComponentsInChildren<Transform>())
            if (element.gameObject.tag == "Projectile")
                from = element.position;
        GameObject expl = null;
        if (Player.explotions.Count > 0)
            expl = Player.explotions.Find(s => !s.activeSelf);
        if (!expl)
        {
            if (Player.explotions.Count < 128)
            {
                expl = Instantiate(Camera.main.GetComponent<Player>().explotion, from, Quaternion.identity, proj.transform.parent);
                Player.explotions.Add(expl);
            }
            else expl = Player.explotions[Player.explotions.Count - 1];
        }
        expl.SetActive(true);
        expl.transform.position = from;
        expl.GetComponent<Explotion>().damage = new Damage(0f, damage1._lightning * 3 + damage1._physical * 3 + damage1._fire * 3 + damage1._void * 3 + damage1._cold * 3, 0f, 0f, 0f);
        expl.GetComponent<Renderer>().material.color = new Color(0f, 0.15f, 1f, 0.6f);
        expl.transform.localScale = new Vector3(5f, 5f, 5f);
        Player.instance.snow.Play();

    }
}
