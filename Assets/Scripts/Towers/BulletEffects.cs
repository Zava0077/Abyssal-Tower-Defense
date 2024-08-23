using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public delegate void BulletEffect(Projectile proj, ref Entity target);

public class BulletEffects : MonoBehaviour
{
    public Projectile _proj;
    #region Missle
    public static BulletEffect missleStart = (Projectile proj, ref Entity target) =>
    {
        if (Player.instance.shoot.isPlaying) Player.instance.shoot.Stop();
            Player.instance.shoot.Play();
    };
    public static BulletEffect missleTravel = (Projectile proj, ref Entity target) =>
    {
        if (proj.liveTime > 5f)
            Destroy(proj);
        if (proj.liveTime > proj.timeNeed)
            proj.projHeight = -50;
        else
        if (proj.liveTime > proj.timeNeed / 2 && proj.projHeight > 0)
            proj.projHeight *= -1;
        proj.transform.position += proj.transform.forward * proj.projSpeed * Time.deltaTime;
        proj.transform.position += new Vector3(0, proj.projHeight * Time.deltaTime, 0);
    };
    #endregion
    #region Laser
    public static BulletEffect laserStart = (Projectile proj, ref Entity target) =>
    {
        proj.collidable = false;
        proj.GetComponent<Collider>().enabled = false;
        proj.waitCast = true;
        if (Player.instance.laser.isPlaying) Player.instance.laser.Stop();
        Player.instance.laser.Play();
    };
    public static BulletEffect laserTravel = (Projectile proj, ref Entity target) =>
    {
        if (proj.liveTime > 0.08f)
            proj.GetComponent<Collider>().enabled = true;
        if (proj.liveTime > 1.5f)
        {
            Destroy(proj.gameObject);
        }
        proj.transform.position += proj.transform.forward * Vector3.Distance(proj.position, proj.target) / 10;
        proj.transform.position = new Vector3(proj.transform.position.x, 1f, proj.transform.position.z);
        proj.transform.rotation.SetEulerRotation(0f, proj.transform.rotation.y, 0f);
    };
    #endregion
    #region Homing
    public static BulletEffect homingStart = (Projectile proj, ref Entity target) =>
    {
        target = Tower.twr.FindEnemy(proj.gameObject, proj.agroRadius, new Dictionary<float, Entity>(), proj.prevEnemy);
        proj.GetComponent<Collider>().enabled = false;
        proj.collidable = false;
        proj.waitCast = true;
        if (Player.instance.laser.isPlaying)
            Player.instance.laser.Stop();
        Player.instance.laser.Play();
        Player.instance.homing.Play();
    };
    public static BulletEffect homingTravel = (Projectile proj,ref Entity target) =>
    {
        if (!target)
            target = Tower.twr.FindEnemy(proj.gameObject, proj.agroRadius, new Dictionary<float, Entity>(), proj.prevEnemy);
        if (proj.liveTime > (2.5f / proj.projSpeed) * 35f)
            Destroy(proj.gameObject);
        if (proj.liveTime > (0.15f / proj.projSpeed) * 35f && target)
        {
            proj.transform.rotation = Quaternion.Lerp(proj.transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(proj.transform.forward, target.transform.position - proj.transform.position, 3.14f, 0f)), (0.02f * 35f) / proj.projSpeed);
            proj.GetComponent<Collider>().enabled = true;
        }
        proj.transform.position += proj.transform.forward * proj.projSpeed * Time.deltaTime;
        proj.transform.position = new Vector3(proj.transform.position.x, 1f, proj.transform.position.z);
    };
    public static BulletEffect homingEnd = (Projectile proj, ref Entity target) =>
    {
        Player.instance.homing.Stop();
    };
    #endregion
    #region Ignite
    public static BulletEffect igniteStart = (Projectile proj, ref Entity target) =>
    {
        Damage damage = proj.damage;
        proj.damage = new Damage(damage._fire / 3, damage._cold / 3, damage._lightning / 3, damage._void / 3, damage._physical / 3);

    };
    public static BulletEffect igniteEnd = (Projectile proj, ref Entity target) =>
    {
        Vector3 from = proj.transform.position;
        Damage damage1 = proj.damage;
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
        expl.GetComponent<Explotion>().damage = new Damage(damage1._lightning * 3 + damage1._physical * 3 + damage1._fire * 3 + damage1._void * 3 + damage1._cold * 3, 0f, 0f, 0f, 0f);
        expl.GetComponent<Renderer>().material.color = new Color(1f, 0.35f, 0f, 0.6f);
        expl.transform.localScale = new Vector3(5f, 5f, 5f);
        if (Player.instance.hot.isPlaying) Player.instance.hot.Stop();
        Player.instance.hot.Play();
    };

    #endregion
    #region Cold
    public static BulletEffect coldStart = (Projectile proj, ref Entity target) =>
    {
        Damage damage = proj.damage;
        proj.damage = new Damage(damage._fire / 3, damage._cold / 3, damage._lightning / 3, damage._void / 3, damage._physical / 3);
    };
    public static BulletEffect coldEnd = (Projectile proj, ref Entity target) =>
    {
        Vector3 from = proj.transform.position;
        Damage damage1 = proj.damage;
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
        if (Player.instance.snow.isPlaying) Player.instance.snow.Stop();
        Player.instance.snow.Play();
    };
    #endregion
    #region Elec
    public static BulletEffect elecStart = (Projectile proj, ref Entity target) =>
    {
        Transform transformChildren = proj.GetComponentInChildren<Transform>();
        Vector3 positionMemory = proj.transform.position;
        transformChildren.localScale = new Vector3(transformChildren.localScale.x, transformChildren.localScale.y, Vector3.Distance(positionMemory, proj.targetMemory));
        transformChildren.position = Vector3.MoveTowards(positionMemory, proj.targetMemory, Vector3.Distance(positionMemory, proj.targetMemory) / 2);
        transformChildren.rotation = Quaternion.LookRotation(Vector3.RotateTowards(proj.transform.right, proj.targetMemory - proj.transform.position, 3.14f, 0));
        proj.GetComponent<Collider>().enabled = false;
        proj.collidable = false;
        Damage damage = proj.GetComponent<Projectile>().damage;
        proj.GetComponent<Projectile>().damage = new Damage(damage._fire / 3, damage._cold / 3, damage._lightning / 3, damage._void / 3, damage._physical / 3);

    };
    public static BulletEffect elecTravel = (Projectile proj, ref Entity target) =>
    {
        if (proj.liveTime > 0.2f)
            proj.GetComponent<Collider>().enabled = true;
        if (proj.liveTime > 0.25f)
        {
            proj.onEnd(proj, ref target);
            Destroy(proj.gameObject);
        }
    };
    public static BulletEffect elecEnd = (Projectile proj, ref Entity target) =>
    {
        Vector3 from = proj.transform.position;
        Damage damage1 = proj.damage;
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
        expl.GetComponent<Explotion>().damage = new Damage(0f, 0f, damage1._lightning * 3 + damage1._physical * 3 + damage1._fire * 3 + damage1._void * 3 + damage1._cold * 3, 0f, 0f);
        expl.GetComponent<Renderer>().material.color = new Color(0f, 0.35f, 1f, 0.6f);
        expl.transform.localScale = new Vector3(5f, 5f, 5f);
    };

    #endregion
    #region Bounce
    public static BulletEffect bounceEnd = (Projectile proj, ref Entity target) =>
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(0, 99);
        if (testrnd < proj.chance.bounce)//заменить на шанс от башни
        {
            Vector3 from = proj.transform.position;
            foreach (var element in proj.GetComponentsInChildren<Transform>())
                if (element.gameObject.tag == "Projectile")
                    from = element.position;
            Entity nextEnemy = Tower.twr.FindEnemy(proj.gameObject, proj.agroRadius, new Dictionary<float, Entity>(), proj.prevEnemy);//иногда баунс всё равно может считать противником самого себя
            Vector3 nextTarget = nextEnemy ? nextEnemy.GetComponent<Transform>().position : Vector3.zero;
            if (nextEnemy == null || (proj.prevEnemy != null && proj.prevEnemy.Count > 0 && nextTarget == proj.prevEnemy[0].gameObject.transform.position)) //
            {
                return;
            }
            Tower.twr.Shoot(from, nextTarget, proj.damage, proj.gameObject, proj.agroRadius, proj.agroRadius, proj.chance,
                proj.onStart, proj.travel, proj.onEnd, proj.projSpeed, proj.transform, proj.prevEnemy);
            if (Player.instance.bounce.isPlaying) Player.instance.bounce.Stop();

            Player.instance.bounce.Play();
        }
    };
    #endregion
    #region Fraction
    public static BulletEffect fractionEnd = (Projectile proj, ref Entity target) =>
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(0, 99);
        if (testrnd < proj.chance.shatter)//заменить на шанс от башни
        {
            for (int i = 0; i < 2; i++)
            {
                bool _elec = false;
                int modifier = 1;
                Vector3 from = proj.transform.position;
                foreach (var element in proj.GetComponentsInChildren<Transform>())
                    if (element.gameObject.tag == "Projectile")
                        from = element.position;
                if (proj.onStart - elecStart != proj.onStart)
                {
                    _elec = true;
                    modifier = 2;
                }

                Vector3 nextTarget = new Vector3(from.x + random.Next(-9 * modifier, 9 * modifier), from.y, from.z + random.Next(-9 * modifier, 9 * modifier));
                Chances newChance = new Chances(proj.chance.bounce, proj.chance.splash, proj.chance.puddle,
                    proj.chance.shatter / 2f, proj.chance.doubleAttack, proj.chance.crit,
                    proj.chance.status, proj.chance.pierce);
                Tower.twr.Shoot(from, nextTarget, new Damage(proj.damage._fire / 2, proj.damage._cold / 2,
                    proj.damage._lightning / 2, proj.damage._void / 2, proj.damage._physical / 2), proj.gameObject, proj.agroRadius, 3f,
                    newChance, proj.onStart,proj.travel,proj.onEnd, proj.projSpeed, proj.transform, !_elec ? null : proj.prevEnemy,
                    new Vector3(proj.transform.localScale.x / 1.5f, proj.transform.localScale.y / 1.5f, proj.transform.localScale.z / 1.5f));
            }
            if (Player.instance.fraction.isPlaying) Player.instance.fraction.Stop();
            Player.instance.fraction.Play();
        }
    };
    #endregion
    #region Explotion
    public static BulletEffect explotionEnd = (Projectile proj, ref Entity target) =>
    {
        System.Random random = new System.Random();
        Projectile _proj = proj.GetComponent<Projectile>();
        float sound = UnityEngine.Random.Range(1, 3);
        float testrnd = random.Next(0, 99);
        if (testrnd < _proj.chance.splash)//
        {
            float size = 0;
            Vector3 from = proj.transform.position;
            GameObject expl = null;
            foreach (var element in proj.GetComponentsInChildren<Transform>())
                if (element.gameObject.tag == "Projectile")
                    from = element.position;
            foreach (var damage in _proj.damage.GetType().GetFields())//
                size += (float)damage.GetValue(_proj.damage) / 7;
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
            expl.GetComponent<Renderer>().material.color = new Color(1, 0.08f, 0f, 0.6f);
            expl.GetComponent<Explotion>().damage = new Damage(15f, 0f, 0f, 0f, 50f);
            expl.transform.localScale = new Vector3(5f + size, 5f + size, 5f + size);
            expl.GetComponent<Explotion>().producer = proj.GetComponent<Projectile>();
            if (sound == 1)
            {
                if (Player.instance.expl.isPlaying)
                    Player.instance.expl.Stop();
                Player.instance.expl.Play();
            }
            else
            {
                if (Player.instance.expl2.isPlaying)
                    Player.instance.expl2.Stop();
                Player.instance.expl2.Play();
            }
        }
    };
    #endregion
    #region Puddle
    public static BulletEffect puddleEnd = (Projectile proj, ref Entity target) =>
    {
        System.Random random = new System.Random();
        float testrnd = random.Next(0, 99);

        if (testrnd < proj.chance.puddle)
        {
            float size = 0;
            float[] colors = new float[3];
            GameObject pudd = null;
            colors[0] = (proj.damage._fire + proj.damage._physical < 255 ? proj.damage._fire + proj.damage._physical : 255) / 255;
            colors[1] = (proj.damage._lightning + proj.damage._void < 255 ? proj.damage._lightning + proj.damage._void : 255) / 255;
            colors[2] = (proj.damage._cold < 255 ? proj.damage._cold : 255) / 255;
            for (int i = 0; i < colors.Length; i++)
                if (colors[i] == colors.Max())
                    colors[i] = 1;
                else colors[i] = colors[i] / colors.Max();
            Vector3 from = proj.transform.position;
            foreach (var element in proj.GetComponentsInChildren<Transform>())
                if (element.gameObject.tag == "Projectile")
                    from = element.position;
            foreach (var damage in proj.damage.GetType().GetFields())//
                size += (float)damage.GetValue(proj.damage) / 4;
            GameObject[] ground = GameObject.FindGameObjectsWithTag("Ground");
            Vector3 puddPosition = new Vector3(from.x, ground[0].transform.position.y, from.z);
            if (Player.puddles.Count > 0) 
                pudd = Player.puddles.Find(s => !s.activeSelf);
            if (!pudd)
            {
                if (Player.puddles.Count < 128)
                {
                    pudd = Instantiate(Camera.main.GetComponent<Player>().puddle, from, Quaternion.identity, proj.transform.parent);
                    Player.puddles.Add(pudd);
                }
                else pudd = Player.puddles[Player.puddles.Count - 1].gameObject;
            }
            pudd.SetActive(true);
            pudd.transform.position = puddPosition;
            pudd.GetComponent<Puddle>().damage = proj.damage;
            pudd.GetComponent<Renderer>().material.color = new Color(colors[0], colors[1], colors[2], 0.6f);
            pudd.transform.localScale = new Vector3(size, pudd.transform.localScale.y, size);
            pudd.GetComponent<Puddle>().producer = proj.GetComponent<Projectile>();
            if (Player.instance.pudd.isPlaying) Player.instance.pudd.Stop();
            Player.instance.pudd.Play();
        }

    };
    #endregion
}
