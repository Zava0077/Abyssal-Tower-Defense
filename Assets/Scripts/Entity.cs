using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public delegate void MobDelete(Entity sender);
public interface IDamagable //не нужный интерфейс
{
    void GetDamage(Damage damage);
}
public interface ITeam
{
    int TeamId { get; set; }
}
public interface IShootable
{
    GameObject Producer { get; set; }
    ProducerSource Source { get; set; }
    void Shoot<T>(T producer, Vector3 turret, Vector3 target,float projSpeed, Projectile missle, Chances chances,
        Action<Projectile> onStart, Action<Projectile> travel, Action<Projectile> onEnd, 
        [Optional] List<Entity> prevEnemy, [Optional] Vector3 scale, [Optional] Damage nDamage) where T : MonoBehaviour, ITeam, ITagger;
}
public interface ITagger
{
    string[] Tags { get; set; }
}
public class Entity : MonoBehaviour, IDamagable, ITeam, IShootable, ITagger
{
    public ProducerSource Source { get; set; }
    public GameObject Producer { get; set; }
    public Color shotShadowColor;
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
    [SerializeField] private string[] forcedTags;
    public string[] Tags { get; set; } = new string[0];
    public Chances chance;
    [SerializeField] private Material damageMat;
    private Color defaultColor;

    public int firstUp;
    public int secondUp;
    public float speed = 0f;
    [SerializeField] private int _forcedTeamId = -1;
    public virtual Vector3 Direction { get; } = Vector3.zero;
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
        entities.Add(this);
        if (_forcedTeamId != -1) TeamId = _forcedTeamId;
        if (forcedTags.Length != 0) Tags = forcedTags;
        if (renderer)
            defaultColor = renderer.materials[0].color;
    }
    private void Death()
    {
        onEntityDeath?.Invoke(this);
        entities.Remove(this);
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
    public virtual void Shoot<T>(T producer, Vector3 turret, Vector3 target, float projSpeed, Projectile missle, Chances chances, Action<Projectile> onStart, Action<Projectile> travel, Action<Projectile> onEnd, [Optional] List<Entity> prevEnemy, [Optional] Vector3 scale,[Optional] Damage nDamage) where T : MonoBehaviour, ITeam, ITagger
    {
        //Чтобы сменить модель можно поменять меш, но для этого нужно все существующие модели заменить на obj модели   
        //Профайлер показывает как трудоёмий процесс. Необходима оптимизация. *
        nProjectile.PullObject(missle, turret, missle.pMesh, false, false).MoveNext();
        //возможно придётся для каждой башни создавать свой пул проджектайлов
        Projectile _missle = nProjectile.pulledObj;
        _missle.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(missle.transform.forward, (target - turret), 3.14f, 0));
        Projectile pMissle = _missle;
        if (scale != Vector3.zero)
            _missle.transform.localScale = scale;
        pMissle.target = target;
        pMissle.damage = nDamage ?? damage;
        pMissle.TeamId = producer.TeamId;
        pMissle.chance = chances;
        pMissle.agroRadius = agroRadius;
        pMissle.prevEnemy = prevEnemy;
        pMissle.projSpeed = projSpeed;
        pMissle.onStart = onStart;
        pMissle.travel = travel;
        pMissle.onEnd = onEnd;
        pMissle.Tags = producer.Tags; //мб не необходимо
        pMissle.liveTime = 0f;
        pMissle.shadowColor = shotShadowColor;
        _missle.gameObject.SetActive(true);
    }
    public Entity FindEnemy<T>(T tower, float agroRadius, Dictionary<float, Entity> enemiesCanShooted, List<Entity> lastEnemy = null) where T : MonoBehaviour, ITeam, ITagger
    {
        List<Entity> enemies = entities;
        List<Entity> resultEnemiesInRange = new List<Entity>();
        foreach (var enemy in enemies)
        {
            if (tower.TeamId == enemy.TeamId || enemy.Tags.Contains(EntityTags.Invisible))
                continue;
            float distance = Vector3.Distance(enemy.transform.position, tower.transform.position);
            enemiesCanShooted.Remove(enemiesCanShooted.FirstOrDefault(s => s.Value == enemy).Key);
            if (distance < agroRadius)
            {
                resultEnemiesInRange.Add(enemy);
                if (enemiesCanShooted.ContainsKey(distance)) distance += 0.0001f;
                enemiesCanShooted[distance] = enemy;
            }
        }
        if (lastEnemy != null)
        {
            lastEnemy.RemoveAll(mob => Vector3.Distance(mob.transform.position, tower.transform.position) > agroRadius);
            if (resultEnemiesInRange.Count == lastEnemy.Count)
            {
                lastEnemy.Clear();
            }
            else
            {
                foreach (var mob in lastEnemy)
                {
                    float distance = Vector3.Distance(mob.transform.position, tower.transform.position);
                    if (resultEnemiesInRange.Contains(mob))
                    {
                        enemiesCanShooted.Remove(distance);
                    }
                }
            }
        }
        if (enemiesCanShooted.Count > 0)
        {
            if (tower.Tags.Contains(EntityTags.Hunter) && enemiesCanShooted.Count > 1)
            {
                var closeEnemies = enemiesCanShooted
                    .Where(pair => Math.Abs(pair.Key - enemiesCanShooted.Keys.Min()) <= 5)
                    .ToList();
                if (closeEnemies.Any())
                {
                    var target = closeEnemies
                        .OrderBy(pair => pair.Value.health)
                        .FirstOrDefault();
                    return target.Value;
                }
            }
            float minDistance = enemiesCanShooted.Keys.Min();
            return enemiesCanShooted[minDistance];
        }
        else
        {
            return null;
        }
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
}
