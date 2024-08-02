using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using static LevelUp;

public class Projectile : MonoBehaviour
{
    public Projectile(Damage damage, Vector3 target, GameObject owner, float agroRadius, Chances chance)
    {
        this.damage = damage;
        this.target = target;
        this.owner = owner;
        this.agroRadius = agroRadius;
        this.chance = chance;
    }
    public Chances chance;
    public Color shadowColor;
    public float agroRadius;
    public GameObject owner;
    public Vector3 target;
    public Vector3 targetMemory;
    public Vector3 projection;
    public Vector3 position;
    public List<Mob> prevEnemy = new List<Mob>();
    public Damage damage;
    public float projSpeed;
    public float archMultiplier;
    public float distance;
    public float projHeight = 0f;
    public float liveTime = 0f;
    public float timeNeed;
    public float testTimer; 
    public bool waitCast = false;
    public bool collidable;
    public List<BulletEffects> effects; //то, что происходит во время полёта, в начале и в конце
    private void Awake()
    {
        projHeight = archMultiplier;
    }
    private void Start()
    {
        if (target != null)
        {
            position = transform.position;
            Vector3 direction = (target - position).normalized;
            distance = Vector3.Distance(position, target);
            //float speed = (Vector3.forward * projSpeed * Time.deltaTime * direction.magnitude).magnitude ;
            float speed = projSpeed * direction.magnitude * 0.85f;
            timeNeed = distance / speed; //0,36 - 0,42
            targetMemory = target;
            testTimer = 0f;
            collidable = true;
        }
        foreach (BulletEffects effect in effects)
            effect.OnStart(gameObject);  //дополнительные эффекты снаряда в начале полета,например, изменение стартового направления, изменение модельки.
    }
    private void Update()
    {
        liveTime += Time.deltaTime; 
        if (waitCast)
            StartCoroutine(shadowCaster());
    }
    private void FixedUpdate()
    {
        foreach (BulletEffects effect in effects)
            effect.Travel(gameObject);//дополнительные эффекты снаряда во время полёта,например, за ним остаётся ядовитое облако

    }
    private void OnCollisionStay(Collision collision)
    {
        prevEnemy ??= new List<Mob>();
        if (collidable)
        {
            if (prevEnemy.Contains(collision.gameObject.GetComponent<Mob>()))
                return;
            if (collision.gameObject.tag == "Enemy" && damage != null)
            {
                DoDamage.DealDamage(collision.gameObject.GetComponent<Entity>(), null, damage);
                prevEnemy.Add(collision.gameObject.GetComponent<Mob>());
            }
            if (collision.gameObject.tag != "Effect")
            {
                foreach (BulletEffects effect in effects)
                    effect.End(gameObject);
                if (chance.pierce < Random.Range(1, 100) || collision.gameObject.tag == "Tower\'s Place" || collision.gameObject.tag == "Unpiercable")
                {
                    Destroy(gameObject);
                    enabled = true;
                }
            }
            //Debug.Log(timeNeed.ToString() + " " + liveTime);
            liveTime = 0f;
        }
    }
    public IEnumerator shadowCaster()
    {
        waitCast = false;
        while (gameObject)
        {
            GameObject shadow = null;
            if (Entity.shadows.Count > 0)
                shadow = Entity.shadows.Find(s => !s.activeSelf);
            if (shadow == null && Entity.shadows.Count < 64)
            {
                shadow = Instantiate(Camera.main.GetComponent<Player>().particleShadow, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
                Entity.shadows.Add(shadow);
            }
            else yield return null;
            shadow.transform.position = gameObject.transform.position;
            shadow.transform.rotation = gameObject.transform.rotation;
            shadow.SetActive(true);
            Fading fadingComponent = shadow.GetComponentInChildren<Fading>();
            fadingComponent.liveTime = 0.1f;
            fadingComponent.timer = 0f;
            fadingComponent.color = shadowColor;
            yield return new WaitForNextFrameUnit();
        }
        waitCast = true;
    }
}
