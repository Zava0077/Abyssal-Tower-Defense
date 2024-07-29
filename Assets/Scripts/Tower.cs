
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using static LevelUp;
using static UnityEngine.GraphicsBuffer;



public class Tower : Entity
{
    public GameObject missle;
    static GameObject[] enemies;
    Entity enemy;
    [SerializeField] GameObject tower;
    public float time;
    public float incDamage = 1;
    public float incAttackSpeed;
    public float incHealth;
    private float currentRotationAngle = 0f;
    public float levelUpsRemain = 2220f;
    public Sprite spriteButton;
    public bool updateLvlUp = true;
    public Resources cost;
    [SerializeField] List<int> costs = new List<int>();
    [SerializeField] List<int> chances = new List<int>();
    [SerializeField] Dictionary<GameObject, bool> keyValuePairs = new Dictionary<GameObject, bool>();
    public List<BulletEffects> effects = new List<BulletEffects>();
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
    public List<LevelUpCallback> levelUpCallbacks = new List<LevelUpCallback>();
    [SerializeField] string[] levelUps;
    public Dictionary<LevelUpCallback, Sprite> levelUpCallbackNames;
    public static Tower twr;
    public Tower()
    {
        twr = this;
    }
    public Chances chance;

    protected void Awake()
    {
        base.Awake();
        levelUpCallbackNames = new Dictionary<LevelUpCallback, Sprite>()
        {
        { FireUp , Camera.main.GetComponent<Player>().levelUpSprites[0] },
        { ColdUp , Camera.main.GetComponent<Player>().levelUpSprites[1] },
        { LightningUp , Camera.main.GetComponent<Player>().levelUpSprites[2] },
        { VoidUp , Camera.main.GetComponent<Player>().levelUpSprites[3] },
        { PhysUp , Camera.main.GetComponent<Player>().levelUpSprites[4] },
        {RangeUp, Camera.main.GetComponent<Player>().levelUpSprites[5] },
        {AttackSpUp, Camera.main.GetComponent<Player>().levelUpSprites[6] },
        {DoubleAttackUp, Camera.main.GetComponent<Player>().levelUpSprites[7] },
        {FractionUp , Camera.main.GetComponent<Player>().levelUpSprites[8] },
        {SplashUp, Camera.main.GetComponent<Player>().levelUpSprites[9] },
        {BounceUp, Camera.main.GetComponent<Player>().levelUpSprites[10] },
        {PuddleUp, Camera.main.GetComponent<Player>().levelUpSprites[11] },
        {FireConvert, Camera.main.GetComponent<Player>().levelUpSprites[12] },
        {ColdConvert, Camera.main.GetComponent<Player>().levelUpSprites[13] },
        {LightningConvert, Camera.main.GetComponent<Player>().levelUpSprites[14] },
        {PhysicalConvert, Camera.main.GetComponent<Player>().levelUpSprites[15] },
        {ProjectileSpeedDown, Camera.main.GetComponent<Player>().levelUpSprites[16] },
        {ProjectileSpeedUp, Camera.main.GetComponent<Player>().levelUpSprites[17] },
        };
        foreach (var levelUp in levelUps)
            levelUpCallbacks.Add(lUCLinks[levelUp]);
        if (!tower)
            tower = GetComponent<GameObject>();

        cost = new Resources(costs[0], costs[1], costs[2], costs[3]);
        chance = new Chances(chances[0], chances[1], chances[2], chances[3], chances[4], chances[5], chances[6], chances[7]);
    }
    protected void Update()
    {
        foreach(var value in keyValuePairs)
        {
            if (value.Value)
            {
                value.Key.active = true;
            }
        }
        //enemiesCanShooted.Remove(enemiesCanShooted.FirstOrDefault(x => x.Value == null).Key);
        currentRotationAngle += attackSpeed * Time.deltaTime * 30;
        if (currentRotationAngle >= 360f)
        {
            currentRotationAngle -= 360f;
        }
        base.Update();
        Quaternion newDir;
        Entity enemy = FindEnemy(tower, tower.GetComponent<Tower>().agroRadius);
        if (enemy)
        {
            time += Time.deltaTime;
            newDir = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (enemy.gameObject.transform.position - tower.transform.position), 3.14f, 0)), attackSpeed);

            if (time > 1 / attackSpeed)
            {
                //Shoot(this.gameObject, enemy.GetComponent<Transform>().position, damage, missle, agroRadius);
                Vector3 fromWhere = gameObject.transform.position;
                foreach (var element in gameObject.GetComponentsInChildren<Transform>())
                {
                    if (element.tag == "Projectile")
                        fromWhere = element.position;
                }
                Shoot(fromWhere, enemy.GetComponent<Transform>().position, damage, missle, agroRadius,missle.GetComponent<Projectile>().archMultiplier, chance, effects, projSpeed, gameObject.transform, new List<Mob>());
                if (Random.Range(1,99) > chance.doubleAttack) //реализация двойной атаки
                    time = 0f;
            }
        }
        else
            newDir = Quaternion.Lerp(transform.rotation,  Quaternion.Euler(0, currentRotationAngle, 0), 0.05f);
        transform.rotation = newDir;
    }
    public Entity FindEnemy(GameObject tower, float agroRadius, List<Mob> lastEnemy = null)
    {
        Dictionary<float, Entity> enemiesCanShooted = new Dictionary<float, Entity>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); //мб переделать, а то слишком дохуя чекать
        bool loop = true;
        bool secLoop = true;
        while (loop)
        {
            int enemyCount = 0;
            foreach (var enemy in enemies)
                if (Vector3.Distance(enemy.transform.position, tower.transform.position) < agroRadius)
                    enemyCount++;
            while(secLoop)
            {
                secLoop = false;
                if (lastEnemy != null)
                    foreach (var enemy in lastEnemy)
                        if (Vector3.Distance(enemy.transform.position, tower.transform.position) > agroRadius)
                        {
                            lastEnemy.Remove(enemy);
                            secLoop = true;
                            break;
                        }

            }
            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(enemy.transform.position, tower.transform.position) < agroRadius)
                {
                    if (lastEnemy != null && enemyCount == lastEnemy.Count && loop)
                    {
                        lastEnemy.Clear();
                        loop = false;
                    }
                    if (lastEnemy != null && lastEnemy.Contains(enemy.GetComponent<Mob>())) continue;
                    if (!enemiesCanShooted.ContainsValue(enemy.GetComponent<Mob>()) || !enemiesCanShooted.ContainsKey(Vector3.Distance(enemy.transform.position, tower.transform.position)))
                        enemiesCanShooted.TryAdd(Vector3.Distance(enemy.transform.position, tower.transform.position), enemy.GetComponent<Mob>());
                    else
                    {
                        enemiesCanShooted.Remove(enemiesCanShooted.FirstOrDefault(x => x.Value == enemy.GetComponent<Mob>()).Key);
                        enemiesCanShooted.Add(Vector3.Distance(enemy.transform.position, tower.transform.position), enemy.GetComponent<Mob>());
                    }
                }
                else if (enemy != enemies[enemies.Length - 1]) continue;
                else
                    if (lastEnemy != null && enemiesCanShooted.Count == 0 && lastEnemy.Count > 0)
                    lastEnemy.Clear();
                loop = false;
            }

        }
        return enemiesCanShooted.Count > 0 ? enemiesCanShooted[enemiesCanShooted.Keys.Min()] : null;
    }

    //public void Shoot(GameObject turret, Vector3 target, Damage damage, GameObject missle, float agroRadius)
    //{
    //    Quaternion rotation = new Quaternion(turret.transform.rotation.x, turret.transform.rotation.y, turret.transform.rotation.z, turret.transform.rotation.w);
    //    rotation.SetEulerAngles((3.14f / 180) * rotation.eulerAngles.x, (3.14f / 180) * rotation.eulerAngles.y - (3.14f / 180) * 90, (3.14f / 180) * rotation.eulerAngles.z);
    //    Vector3 projectile = Vector3.forward;
    //    foreach (var element in turret.GetComponentsInChildren<Transform>())
    //    {
    //        if (element.tag == "Projectile")
    //            projectile = element.position;
    //    }
    //    GameObject _missle = Instantiate(missle, projectile, Quaternion.identity, turret.transform.parent);
    //    _missle.transform.rotation = rotation;
    //    _missle.GetComponent<Projectile>().target = target;
    //    _missle.GetComponent<Projectile>().damage = damage;
    //    //_missle.GetComponent<Projectile>().owner = turret;
    //    _missle.GetComponent<Projectile>().chance = chance;
    //    _missle.GetComponent<Projectile>().agroRadius = agroRadius;
    //    if (turret.GetComponent<Tower>())
    //        _missle.GetComponent<Projectile>().effects = turret.GetComponent<Tower>().effects;
    //    if (turret.GetComponent<Tower>())
    //        _missle.GetComponent<Projectile>().projSpeed = turret.GetComponent<Tower>().projSpeed;
    //}
    public void Shoot(Vector3 turret, Vector3 target, Damage damage, GameObject missle, float agroRadius,float archMultiplier, Chances chances, List<BulletEffects> effects, float projSpeed, Transform parent, [Optional] List<Mob> prevEnemy, [Optional] Vector3 scale)
    {
        GameObject _missle = Instantiate(missle, turret, Quaternion.LookRotation(Vector3.RotateTowards(missle.transform.forward, (target - turret), 3.14f, 0)), parent.transform.parent);
        if (scale != Vector3.zero)
            _missle.transform.localScale = scale;
        _missle.GetComponent<Projectile>().target = target;
        _missle.GetComponent<Projectile>().damage = damage;
        _missle.GetComponent<Projectile>().archMultiplier = archMultiplier;
        _missle.GetComponent<Projectile>().chance = chances;
        _missle.GetComponent<Projectile>().agroRadius = agroRadius;
        _missle.GetComponent<Projectile>().prevEnemy = prevEnemy;
        _missle.GetComponent<Projectile>().projSpeed = projSpeed;
        _missle.GetComponent<Projectile>().effects.Clear();
        foreach (var effect in effects)
            _missle.GetComponent<Projectile>().effects.Add(effect.Clone() as BulletEffects);;
        _missle.GetComponent<Projectile>().liveTime = 0f;
    }
}
