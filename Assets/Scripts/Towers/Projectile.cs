using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static LevelUp;

public sealed class Projectile : MonoBehaviour, ITeam, IShootable, IMeshHolder
{
    public Projectile(Damage damage, Vector3 target, GameObject owner, float agroRadius, Chances chance)
    {
        this.damage = damage;
        this.target = target;
        this.owner = owner;
        this.agroRadius = agroRadius;
        this.chance = chance;
    }
    public MeshHolder MeshHolder { get; set; }
    public Mesh pMesh;

    private bool hasCollided = false;
    public int TeamId { get; set; }
    public Chances chance;
    public Color shadowColor;
    public float agroRadius;
    public Entity followTarget;
    public GameObject owner;
    public Vector3 target;
    public Vector3 targetMemory;
    public Vector3 projection;
    public Vector3 position;
    public List<Entity> prevEnemy = new List<Entity>();
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
    }
    private void OnDisable()
    {
        Entity.onEntityDeath -= OnEntityDeath;
    }
    private void OnEnable()
    {
        if (target != null)
        {
            position = transform.position;
            projHeight = 0f;
            liveTime = 0f;
            Vector3 direction = (target - position).normalized;
            distance = Vector3.Distance(position, target);
            float speed = projSpeed * direction.magnitude * 0.85f;
            timeNeed = distance / speed;
            targetMemory = target;
            testTimer = 0f;
            collidable = true;
        }
        Entity.onEntityDeath += OnEntityDeath;
        onStart?.Invoke(this);
        hasCollided = false;
    }
    private void OnEntityDeath(Entity sender)
    {
        if (prevEnemy != null && prevEnemy.Count > 0)
            prevEnemy.Remove(sender as Entity);
    }
    private void Update()
    {
        liveTime += Time.deltaTime; 
        if (waitCast)
            StartCoroutine(shadowCaster());
    }
    private void FixedUpdate()//дорого. вложенные методы могут быть тяжелыми
    {
        travel?.Invoke(this);
    }
    public void Shoot<T>(T producer, Vector3 turret, Vector3 target, float projSpeed, Projectile missle, Chances chances, BulletEffect onStart, BulletEffect travel, BulletEffect onEnd, [Optional] List<Entity> prevEnemy, [Optional] Vector3 scale, [Optional] Damage nDamage) where T : MonoBehaviour, ITeam
    {
        Entity.entity.Shoot(producer, turret, target,projSpeed, missle, chance, onStart, travel, onEnd, prevEnemy, scale, nDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasCollided) return;
        if (damage == null) throw new ArgumentNullException(nameof(damage));
        prevEnemy ??= new List<Entity>();
        Entity otherEntity = other.GetComponent<Entity>();
        if (other.gameObject.tag == "Effect") return;
        if (otherEntity != null && (prevEnemy.Contains(otherEntity) || otherEntity.TeamId == TeamId)) return;
        if (otherEntity)
        {
            otherEntity.GetDamage(damage);
            prevEnemy.Add(otherEntity);
        }
        onEnd?.Invoke(this);
        if (chance.pierce < UnityEngine.Random.Range(1, 100) || other.gameObject.tag == "Tower\'s Place" || other.gameObject.tag == "Unpiercable")
        {
            if (Player.instance.hit.isPlaying) Player.instance.hit.Stop();
            Player.instance.hit.Play();
            gameObject.SetActive(false);//иногда не регает
            enabled = true;
            hasCollided = true;
        }
        else
        {
            if (Player.instance.pierce.isPlaying) Player.instance.pierce.Stop();
            Player.instance.pierce.Play();
        }
        liveTime = 0f;
    }
    public IEnumerator shadowCaster()
    {
        waitCast = false; 
        Fading fadingComponent;
        Fading pref = Player.instance.particleShadow.GetComponentInChildren<Fading>();
        while (gameObject)
        {
            Player.nShadows.PullObject(pref, gameObject.transform.position, null, true, true, 1).MoveNext();
            fadingComponent = Player.nShadows.pulledObj;
            fadingComponent.color = shadowColor;
            fadingComponent.transform.rotation = gameObject.transform.rotation;
            fadingComponent.liveTime = 0.1f;
            yield return new WaitForNextFrameUnit();
        }
        waitCast = true;
    }
}
