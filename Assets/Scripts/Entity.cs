using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public delegate void MobDelete(Entity sender);
public interface IDamagable
{
    void GetDamage(Damage damage);
}
public interface ITeam
{
    int TeamId { get; set; }
}
internal interface IShootable
{
    void Shoot<T>(T producer, Vector3 turret, Vector3 target,float projSpeed, Projectile missle, Chances chances, 
        BulletEffect onStart, BulletEffect travel, BulletEffect onEnd, 
        [Optional] List<Entity> prevEnemy, [Optional] Vector3 scale, [Optional] Damage nDamage) where T : MonoBehaviour, ITeam;
}
public class Entity : MonoBehaviour, IDamagable, ITeam, IShootable
{
    public static Entity entity;
    public static List<Entity> entities = new List<Entity>();
    public static event MobDelete onEntityDeath;
    protected ObjectPool<Projectile> nProjectile = new ObjectPool<Projectile>(256);
    public static List<GameObject> shadows = new List<GameObject>();
    [Header("Stats")] //вывести статы в отдельный класс
    public float maxHealth;
    public float health; 
    public float attackSpeed;
    public float projSpeed;
    public float multiplierTakeDamage;
    public float agroRadius;
    public Damage damage;
    public Resistances resistances;
    public List<Status> statuses = new List<Status>();
    public List<float> _damage = new List<float>();
    public List<float> _resist = new List<float>();
    private Renderer renderer;
    public Chances chance;
    [SerializeField] private Material damageMat;
    private Color defaultColor;

    public int firstUp;
    public int secondUp;
    public int TeamId { get; set; }
    public Entity()
    {
        entity = this;
    }

    public void Awake()
    {
        damage = new Damage(_damage[0], _damage[1], _damage[2], _damage[3], _damage[4]);
        resistances = new Resistances(_resist[0], _resist[1], _resist[2], _resist[3], _resist[4]);
        renderer = GetComponent<Renderer>();

        if (renderer)
            defaultColor = renderer.materials[0].color;
    }
    private void Death()
    {
        onEntityDeath?.Invoke(this);
        Destroy(gameObject);
    }
    public void Update()
    {
        foreach (Status status in statuses)
            status.DoStatus();

        if (health <= 0)
            Death();
    }
    public IEnumerator ColorChanger()
    {
        if (renderer)
        {
            renderer.materials[0].color = new Color(255, 0, 0, 75);
            yield return new WaitForSeconds(0.1f);
            renderer.materials[0].color = defaultColor;
        }
    }
    public virtual void Shoot<T>(T producer, Vector3 turret, Vector3 target, float projSpeed, Projectile missle, Chances chances, BulletEffect onStart, BulletEffect travel, BulletEffect onEnd, [Optional] List<Entity> prevEnemy, [Optional] Vector3 scale,[Optional] Damage nDamage) where T : MonoBehaviour, ITeam
    {
        //Чтобы сменить модель можно поменять меш, но для этого нужно все существующие модели заменить на obj модели   
        //Профайлер показывает как трудоёмий процесс. Необходима оптимизация. *
        nProjectile.PullObject(missle, turret, missle.pMesh, false, false).MoveNext();//тут меняет демедж
        //возможно придётся для каждой башни создавать свой пул проджектайлов
        Projectile _missle = nProjectile.pulledObj;
        _missle.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(missle.transform.forward, (target - turret), 3.14f, 0));
        Projectile pMissle = _missle;
        if (scale != Vector3.zero)
            _missle.transform.localScale = scale;
        pMissle.target = target;
        pMissle.damage = nDamage != null ? nDamage : damage;
        pMissle.TeamId = producer.TeamId;
        pMissle.chance = chances;
        pMissle.agroRadius = agroRadius;
        pMissle.prevEnemy = prevEnemy;
        pMissle.projSpeed = projSpeed;
        pMissle.onStart = onStart;
        pMissle.travel = travel;
        pMissle.onEnd = onEnd;
        pMissle.liveTime = 0f;
        _missle.gameObject.SetActive(true);
    }
    public void GetDamage(Damage damage) //когда моб умирает иногда всё равно вызывается
    {
        health -= damage._fire * (1 - resistances._fire);
        health -= damage._lightning * (1 - resistances._lightning);
        health -= damage._cold * (1 - resistances._cold);
        health -= damage._void * (1 - resistances._void);
        health -= damage._physical * (1 - resistances._physical);
        StartCoroutine(ColorChanger());
    }

    public Type CheckEntity()
    {
        return this.GetType();
    }
}
