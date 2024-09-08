using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.UIElements;
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
    private Entity followTarget;
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
    public BulletEffect onStart;
    public BulletEffect travel;
    public BulletEffect onEnd;
    private void Awake()
    {
        projHeight = archMultiplier;
        Entity.onEntityDeath += OnEntityDeath;
    }
    private void Start()
    {
        if (target != null)
        {
            position = transform.position;
            Vector3 direction = (target - position).normalized;
            distance = Vector3.Distance(position, target);
            float speed = projSpeed * direction.magnitude * 0.85f;
            timeNeed = distance / speed; 
            targetMemory = target;
            testTimer = 0f;
            collidable = true;
        }
        onStart?.Invoke(this,ref followTarget);
    }
    private void OnEntityDeath(object sender)
    {
        if (sender.GetType() == typeof(Mob) && prevEnemy != null && prevEnemy.Count > 0)
            prevEnemy.Remove((Mob)sender);
    }
    private void Update()
    {
        liveTime += Time.deltaTime; 
        if (waitCast)
            StartCoroutine(shadowCaster());
    }
    private void FixedUpdate()//дорого. внутренние методы могут быть тяжелыми
    {
        travel?.Invoke(this, ref followTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damage == null) throw new ArgumentNullException("Урон неопознан");
        prevEnemy ??= new List<Mob>();
        Entity otherEntity = other.GetComponent<Entity>();
        Mob otherMob = otherEntity as Mob;
        if (other.gameObject.tag == "Effect") return;
        if (prevEnemy.Contains(otherMob)) return;
        if (other.gameObject.tag == "Enemy" && damage != null)
        {
            otherEntity.GetDamage(damage);
            prevEnemy.Add(other.gameObject.GetComponent<Mob>());
        }
        onEnd?.Invoke(this, ref followTarget);
        if (chance.pierce < UnityEngine.Random.Range(1, 100) || other.gameObject.tag == "Tower\'s Place" || other.gameObject.tag == "Unpiercable")
        {
            if (Player.instance.hit.isPlaying) Player.instance.hit.Stop();
            Player.instance.hit.Play();
            Destroy(gameObject);//
            enabled = true;
        }
        else
        {
            if (Player.instance.pierce.isPlaying) Player.instance.pierce.Stop();
            Player.instance.pierce.Play();
        }
        liveTime = 0f;
    }
    public IEnumerator shadowCaster()//хз как это засунуть в дженерик. Потом
    {
        waitCast = false; 
        Fading fadingComponent;
        Fading pref = Player.instance.particleShadow.GetComponentInChildren<Fading>();
        Mesh mesh = pref.GetComponent<Mesh>();
        while (gameObject)
        {
            Player.nShadows.PullObject(pref, gameObject.transform.position, mesh, true, 1).MoveNext();
            fadingComponent = Player.nShadows.pulledObj;
            fadingComponent.color = shadowColor;
            fadingComponent.transform.rotation = gameObject.transform.rotation;
            fadingComponent.liveTime = 0.1f;
            yield return new WaitForNextFrameUnit();
        }
        waitCast = true;
    }
}
