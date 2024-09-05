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
    private static GameObject[] enemies;
    private Entity enemy;
    private float time;
    private float currentRotationAngle = 0f;
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

    Dictionary<float, Entity> enemiesCanShooted = new Dictionary<float, Entity>();
    public Dictionary<string, LevelUpCallback> lUCLinks = new Dictionary<string, LevelUpCallback>()
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
    public Dictionary<string, BulletEffect[]> effectLinks = new Dictionary<string, BulletEffect[]>()
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
        Player player = Camera.main.GetComponent<Player>();
        
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
    new protected void Update()
    {
        foreach(var value in keyValuePairs)
            if (value.Value)
                value.Key.active = true;
        currentRotationAngle += attackSpeed * Time.deltaTime * 30;
        if (currentRotationAngle >= 360f)
        {
            currentRotationAngle -= 360f;
        }
        base.Update();
        Quaternion newDir;
        enemy = FindEnemy(tower, tower.GetComponent<Tower>().agroRadius, enemiesCanShooted);
        if (enemy)
        {
            time += Time.deltaTime;
            newDir = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (enemy.gameObject.transform.position - tower.transform.position), 3.14f, 0)), attackSpeed);

            if (time > 1 / attackSpeed)
            {
                Vector3 fromWhere = gameObject.transform.position;

                foreach (var element in gameObject.GetComponentsInChildren<Transform>())
                    if (element.tag == "Projectile")
                        fromWhere = element.position;

                //Shoot(fromWhere, enemy.transform.position + (enemy as Mob).Direction * (enemy as Mob).speed / (projSpeed/10), damage, missle,
                //    agroRadius,missle.GetComponent<Projectile>().archMultiplier, chance, effects, projSpeed, gameObject.transform, new List<Mob>());
                Shoot(fromWhere, enemy.transform.position + (enemy as Mob).Direction * (enemy as Mob).speed / (projSpeed / 10), damage, missle,
                    agroRadius, missle.GetComponent<Projectile>().archMultiplier, chance, onStart, travel, onEnd, projSpeed, gameObject.transform, new List<Mob>());

                if (UnityEngine.Random.Range(1,99) > chance.doubleAttack)
                    time = 0f;
            }
        }
        else
            newDir = Quaternion.Lerp(transform.rotation,  Quaternion.Euler(0, currentRotationAngle, 0), 0.05f);
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
    public Entity FindEnemy(GameObject tower, float agroRadius, Dictionary<float, Entity> enemiesCanShooted, List<Mob> lastEnemy = null)
    {
        //сменить на изучение статического списка. Который будет изменятся при объявлении нового моба в Awake класса.
        //надо проверить если поместить в конструтор поменяяется ли что-нибудь
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<Mob> currentEnemiesInRange = new List<Mob>();

        foreach (var enemy in enemies)
        {
            Mob mob = enemy.GetComponent<Mob>();
            float distance = Vector3.Distance(enemy.transform.position, tower.transform.position);
            enemiesCanShooted.Remove(enemiesCanShooted.FirstOrDefault(s => s.Value == mob).Key);
            if (distance < agroRadius)
            {
                currentEnemiesInRange.Add(mob);
                enemiesCanShooted[distance] = mob;
            }
        }

        if (lastEnemy != null)
        {
            lastEnemy.RemoveAll(mob => Vector3.Distance(mob.transform.position, tower.transform.position) > agroRadius);
            if (currentEnemiesInRange.Count == lastEnemy.Count)
            {
                lastEnemy.Clear();
            }
            else
            {
                foreach (var mob in lastEnemy)
                {
                    float distance = Vector3.Distance(mob.transform.position, tower.transform.position);
                    if (currentEnemiesInRange.Contains(mob))
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

    public void Shoot(Vector3 turret, Vector3 target, Damage damage, GameObject missle, float agroRadius, float archMultiplier, Chances chances, BulletEffect onStart, BulletEffect travel, BulletEffect onEnd, float projSpeed, Transform parent, [Optional] List<Mob> prevEnemy, [Optional] Vector3 scale)
    {
        //Заменить создание объекта на перетаскивание уже существующего из пула объектов.
        //Чтобы сменить модель можно поменять меш, но для этого нужно все существующие модели заменить на obj модели
        //Создать дженерик класс для быстрого создания пулов
        
        //Профайлер показывает как трудоёмий процесс. Необходима оптимизация.
        GameObject _missle = Instantiate(missle, turret, Quaternion.LookRotation(Vector3.RotateTowards(missle.transform.forward, (target - turret), 3.14f, 0)), parent.transform.parent);
        Projectile pMissle = _missle.GetComponent<Projectile>();
        if (scale != Vector3.zero)
            _missle.transform.localScale = scale;
        pMissle.target = target;
        pMissle.damage = damage;
        pMissle.archMultiplier = archMultiplier;
        pMissle.chance = chances;
        pMissle.agroRadius = agroRadius;
        pMissle.prevEnemy = prevEnemy;
        pMissle.projSpeed = projSpeed;
        pMissle.onStart = onStart;
        pMissle.travel = travel;
        pMissle.onEnd = onEnd;
        pMissle.liveTime = 0f;
    }

}
