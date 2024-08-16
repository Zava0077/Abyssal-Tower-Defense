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
    [SerializeField] private GameObject tower;
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
    public List<LevelUpCallback> levelUpCallbacks = new List<LevelUpCallback>();
    [SerializeField] string[] levelUps;
    public Dictionary<LevelUpCallback, Sprite> levelUpCallbackNames;
    public static Tower twr;
    public Tower()
    {
        twr = this;
    }
    public Chances chance;

    new protected void Awake()
    {
        base.Awake();
        Player player = Camera.main.GetComponent<Player>();

        foreach (var levelUp in levelUps)
            levelUpCallbacks.Add(lUCLinks[levelUp]);

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
   
                Shoot(fromWhere, enemy.transform.position + (enemy as Mob).Direction * (enemy as Mob).speed / (projSpeed/10), damage, missle, agroRadius,missle.GetComponent<Projectile>().archMultiplier, chance, effects, projSpeed, gameObject.transform, new List<Mob>());
                if (Random.Range(1,99) > chance.doubleAttack)
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
            _missle.GetComponent<Projectile>().effects.Add(effect.Clone() as BulletEffects);
        _missle.GetComponent<Projectile>().liveTime = 0f;
    }
}
