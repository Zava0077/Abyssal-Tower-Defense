using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using static LevelUp;
using static BulletEffects;
using static UnityEngine.GraphicsBuffer;

public class Tower : Entity
{
    private Entity enemy;
    private float time;
    private float rotationAngle = 0f;
    private float ResultRotationAngle {
        get => rotationAngle;
        set
        {
            if (value > 360) value -= 360;
            if (value < 0) value += 360;
            rotationAngle = value;
        }
    } 
    [SerializeField] private GameObject tower;
    [SerializeField] private List<int> costs = new List<int>();
    [SerializeField] private List<int> chances = new List<int>();
    [SerializeField] private List<string> starterEffects = new List<string>();
    [SerializeField] private Dictionary<GameObject, bool> keyValuePairs = new Dictionary<GameObject, bool>();
    public GameObject missle;
    public float incDamage = 1;
    public float incAttackSpeed;
    public float incHealth;
    public float levelUpsRemain = 2220f;
    public Sprite spriteButton;
    public bool updateLvlUp = true;
    public Resources cost;
    public BulletEffect onStart;
    public BulletEffect travel;
    public BulletEffect onEnd;
    private Dictionary<float, Entity> enemiesCanShooted = new Dictionary<float, Entity>();
    public static Dictionary<string, LevelUpCallback> lUCLinks = new Dictionary<string, LevelUpCallback>()
    {
        { "FireUp",FireUp },
        { "ColdUp",ColdUp },
        { "LightningUp",LightningUp },
        { "VoidUp",VoidUp },
        { "PhysUp",PhysUp },
        { "RangeUp",RangeUp },
        { "AttackSpUp",AttackSpUp },
        { "DoubleAttackUp",DoubleAttackUp },
        { "FractionUp",FractionUp },
        { "SplashUp",SplashUp },
        { "BounceUp",BounceUp },
        { "PuddleUp",PuddleUp },
        { "FireConvert",FireConvert },
        { "ColdConvert",ColdConvert },
        { "LightningConvert",LightningConvert },
        { "PhysicalConvert",PhysicalConvert },
        { "ProjectileSpeedDown",ProjectileSpeedDown },
        { "ProjectileSpeedUp",ProjectileSpeedUp },
    };
    public static Dictionary<string, BulletEffect[]> effectLinks = new Dictionary<string, BulletEffect[]>()
    {
        { "Missle",new[] {missleStart,missleTravel,null } },
        { "Laser",new[] {laserStart, laserTravel, null } },
        { "Homing",new[] {homingStart,homingTravel,homingEnd } },
        { "Bomb", new[] {null,null,explotionEnd } },
        { "Ignite",new[] { igniteStart, null, igniteEnd } },
        { "Cold",new[] { coldStart, null, coldEnd } },
        { "Elec",new[] { elecStart, elecTravel, elecEnd} },
    };
    public List<LevelUpCallback> levelUpCallbacks = new List<LevelUpCallback>();
    [SerializeField] private string[] levelUps;
    public Dictionary<LevelUpCallback, Sprite> levelUpCallbackNames;
    public static Tower twr;
    public Tower() => twr = this;
    public Chances chance;
    new protected void Awake()
    {
        base.Awake();
        Player player = Player.instance;
        Mesh pMesh = gameObject.GetComponentInChildren<MeshFilter>().mesh; //надо поместить меш в объект и ссылаться на этот объект с мешем, а не на сам меш.
        missle.GetComponent<Projectile>().MeshHolder = new MeshHolder(pMesh);
        foreach (var levelUp in levelUps)
            levelUpCallbacks.Add(lUCLinks[levelUp]);
        foreach(var eff in starterEffects)
        {
            onStart += effectLinks[eff][0];
            travel += effectLinks[eff][1];
            onEnd += effectLinks[eff][2];
        }
        if (!tower)
            tower = GetComponent<GameObject>();
        TeamId = 1;
        entities.Add(this);
        missle.GetComponent<Projectile>().damage = damage;
        levelUpCallbackNames = new Dictionary<LevelUpCallback, Sprite>()
        {
            { FireUp , player.levelUpSprites[0] },
            { ColdUp , player.levelUpSprites[1] },
            { LightningUp , player.levelUpSprites[2] },
            { VoidUp , player.levelUpSprites[3] },
            { PhysUp , player.levelUpSprites[4] },
            { RangeUp, player.levelUpSprites[5] },
            { AttackSpUp, player.levelUpSprites[6] },
            { DoubleAttackUp, player.levelUpSprites[7] },
            { FractionUp , player.levelUpSprites[8] },
            { SplashUp, player.levelUpSprites[9] },
            { BounceUp, player.levelUpSprites[10] },
            { PuddleUp, player.levelUpSprites[11] },
            { FireConvert, player.levelUpSprites[12] },
            { ColdConvert, player.levelUpSprites[13] },
            { LightningConvert, player.levelUpSprites[14] },
            { PhysicalConvert, player.levelUpSprites[15] },
            { ProjectileSpeedDown, player.levelUpSprites[16] },
            { ProjectileSpeedUp, player.levelUpSprites[17] },
        };
        cost = new Resources(costs[0], costs[1], costs[2], costs[3]);
        chance = new Chances(chances[0], chances[1], chances[2], chances[3], chances[4], chances[5], chances[6], chances[7]);
    }
    private void OnDestroy()
    {
        entities.Remove(this);
    }
    private void FixedUpdate()
    {
        enemy = FindEnemy(this, agroRadius, enemiesCanShooted);
    }
    new protected void Update()
    {
        //foreach(var value in keyValuePairs)
        //    if (value.Value)
        //        value.Key.active = true;
        base.Update();
        ResultRotationAngle += attackSpeed * Time.deltaTime * 30;
        Quaternion newDir;
        if (enemy)
        {
            time += Time.deltaTime;
            newDir = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (enemy.gameObject.transform.position - tower.transform.position), 3.14f, 0)), attackSpeed);

            if (time > 1 / attackSpeed)
            {
                Vector3 fromWhere = gameObject.transform.position;
                Projectile pMissle = missle.GetComponent<Projectile>();
                foreach (var element in gameObject.GetComponentsInChildren<Transform>())
                    if (element.tag == "Projectile")
                        fromWhere = element.position;
                Shoot(this, fromWhere, enemy.transform.position + (enemy as Mob).Direction * (enemy as Mob).speed / (projSpeed / 10), pMissle,
                    chance, onStart, travel, onEnd, new List<Entity>(), missle.transform.localScale, pMissle.damage);
                if (UnityEngine.Random.Range(1,99) > chance.doubleAttack)
                    time = 0f;
            }
        }
        else
            newDir = Quaternion.Lerp(transform.rotation,  Quaternion.Euler(0, ResultRotationAngle, 0), 0.05f);
        transform.rotation = newDir;
    }
    private void OnDrawGizmos()
    {
        if(enemy)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(enemy.transform.position, new Vector3(enemy.transform.position.x, enemy.transform.position.y + 10f, enemy.transform.position.z));
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(enemy.transform.position + (enemy as Mob).Direction * (enemy as Mob).speed / (projSpeed / 10),
                new Vector3(enemy.transform.position.x, enemy.transform.position.y + 10f, enemy.transform.position.z) + (enemy as Mob).Direction * (enemy as Mob).speed / (projSpeed / 10));
        }
    }
    //перекинуть эти методы в Entity
    public Entity FindEnemy<T>(T tower, float agroRadius, Dictionary<float, Entity> enemiesCanShooted, List<Entity> lastEnemy = null) where T : MonoBehaviour, ITeam
    {
        List<Entity> enemies = entities;
        List<Entity> resultEnemiesInRange = new List<Entity>();
        
        foreach (var enemy in enemies)
        {
            if (tower.TeamId == enemy.TeamId) //руки мне не отрубайте пж
                continue; 
            float distance = Vector3.Distance(enemy.transform.position, tower.transform.position);
            enemiesCanShooted.Remove(enemiesCanShooted.FirstOrDefault(s => s.Value == enemy).Key);
            if (distance < agroRadius)
            {
                resultEnemiesInRange.Add(enemy);
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
            float minDistance = enemiesCanShooted.Keys.Min();
            return enemiesCanShooted[minDistance];
        }
        else
        {
            return null;
        }
    }
    //public void Shoot<T>(T producer, Vector3 turret, Vector3 target, Projectile missle, Chances chances, BulletEffect onStart, BulletEffect travel, BulletEffect onEnd, [Optional] Damage nDamage,[Optional] List<Entity> prevEnemy, [Optional] Vector3 scale) where T : MonoBehaviour, ITeam
    //{
    //    //Чтобы сменить модель можно поменять меш, но для этого нужно все существующие модели заменить на obj модели   
    //    //Профайлер показывает как трудоёмий процесс. Необходима оптимизация. *
    //    nProjectile.PullObject(missle, turret, missle.pMesh, false).MoveNext();
    //    //возможно придётся для каждой башни создавать свой пул проджектайлов
    //    Projectile _missle = nProjectile.pulledObj;
    //    _missle.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(missle.transform.forward, (target - turret), 3.14f, 0));
    //    Projectile pMissle = _missle;
    //    if (scale != Vector3.zero)
    //        _missle.transform.localScale = scale;
    //    pMissle.target = target;
    //    pMissle.damage = nDamage != null ? nDamage : damage;//поместить в Entity
    //    pMissle.TeamId = producer.TeamId;
    //    pMissle.chance = chances;//поместить в Entity
    //    pMissle.agroRadius = agroRadius;//поместить в Entity
    //    pMissle.prevEnemy = prevEnemy;
    //    pMissle.projSpeed = projSpeed;//поместить в Entity
    //    pMissle.onStart = onStart;//поместить в Entity
    //    pMissle.travel = travel;//поместить в Entity
    //    pMissle.onEnd = onEnd;//поместить в Entity
    //    pMissle.liveTime = 0f;
    //}
    //public void Shoot<T>(Vector3 turret, Vector3 target, Damage damage, GameObject missle, float agroRadius, float archMultiplier, Chances chances,T producer, BulletEffect onStart, BulletEffect travel, BulletEffect onEnd, float projSpeed, Transform parent, [Optional] List<Entity> prevEnemy, [Optional] Vector3 scale) where T : MonoBehaviour, ITeam
    //{
    //    //Заменить создание объекта на перетаскивание уже существующего из пула объектов.
    //    //Чтобы сменить модель можно поменять меш, но для этого нужно все существующие модели заменить на obj модели   
    //    //Профайлер показывает как трудоёмий процесс. Необходима оптимизация. *
    //    //GameObject -> Projectile
    //    GameObject _missle = Instantiate(missle, turret, Quaternion.LookRotation(Vector3.RotateTowards(missle.transform.forward, (target - turret), 3.14f, 0)), parent.transform.parent);
    //    Projectile pMissle = _missle.GetComponent<Projectile>();
    //    if (scale != Vector3.zero)
    //        _missle.transform.localScale = scale;
    //    pMissle.target = target;
    //    pMissle.damage = damage;
    //    pMissle.TeamId = producer.TeamId;
    //    pMissle.archMultiplier = archMultiplier;
    //    pMissle.chance = chances;
    //    pMissle.agroRadius = agroRadius;
    //    pMissle.prevEnemy = prevEnemy;
    //    pMissle.projSpeed = projSpeed;
    //    pMissle.onStart = onStart;
    //    pMissle.travel = travel;
    //    pMissle.onEnd = onEnd;
    //    pMissle.liveTime = 0f;
    //}
}
