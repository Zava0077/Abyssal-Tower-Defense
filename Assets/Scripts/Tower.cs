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
using System.Resources;
using UnityEditor;
using static UnityEngine.EventSystems.EventTrigger;
public static class EntityTags
{
    public static readonly string Invulnerable = "Invulnerable";
    public static readonly string Invisible = "Invisible";
    public static readonly string Hunter = "Hunter";
}
public class Tower : Entity
{
    [SerializeField] private GameObject tower;
    [SerializeField] public List<int> costs = new List<int>();
    [SerializeField] private List<int> chances = new List<int>();
    [SerializeField] private List<string> starterEffects = new List<string>();
    [SerializeField] private string[] levelUps;
    public GameObject missle;
    public float incDamage = 1f;
    public float incAttackSpeed;
    public float incHealth;
    private float levelUpsRemain = 1000f; //какого ху€ float??
    public int currentLevel;
    public float LevelUpsRemain
    {
        get => levelUpsRemain;
        set
        {
            if (value < levelUpsRemain) currentLevel++;
            levelUpsRemain = value;
            GenerateUps();
            CanvasController.ExitButton();
        }
    }
    public Sprite spriteButton;
    public Resources cost;
    public Resources upgradeCost;
    public Action<Projectile> onStart;
    public Action<Projectile> travel;
    public Action<Projectile> onEnd;
    public List<Action<Tower>> levelUpCallbacks = new List<Action<Tower>>();

    public static Dictionary<string, Action<Tower>> lUCLinks = new Dictionary<string, Action<Tower>>()
    {
        { "FireUp", FireUp },
        { "ColdUp", ColdUp },
        { "LightningUp", LightningUp },
        { "VoidUp", VoidUp },
        { "PhysUp", PhysUp },
        { "RangeUp", RangeUp },
        { "AttackSpUp", AttackSpUp },
        { "DoubleAttackUp", DoubleAttackUp },
        { "FractionUp", FractionUp },
        { "SplashUp", SplashUp },
        { "BounceUp", BounceUp },
        { "PuddleUp", PuddleUp },
        { "FireConvert", FireConvert },
        { "ColdConvert", ColdConvert },
        { "LightningConvert", LightningConvert },
        { "PhysicalConvert", PhysicalConvert },
        { "ProjectileSpeedDown", ProjectileSpeedDown },
        { "ProjectileSpeedUp", ProjectileSpeedUp }
    };

    public static Dictionary<string, Action<Projectile>[]> effectLinks = new Dictionary<string, Action<Projectile>[]>()
    {
        { "Missle", new[] { missleStart, missleTravel, null } },
        { "Laser", new[] { laserStart, laserTravel, null } },
        { "Homing", new[] { homingStart, homingTravel, homingEnd } },
        { "Bomb", new[] { null, null, explotionEnd } },
        { "Ignite", new[] { igniteStart, null, igniteEnd } },
        { "Cold", new[] { coldStart, null, coldEnd } },
        { "Elec", new[] { elecStart, elecTravel, elecEnd } },
        { "Fraction", new[] { null, null, fractionEnd } },
        { "Bounce", new[] { null, null, bounceEnd } },
        { "Puddle", new[] { null, null, puddleEnd} }
    };

    public static Dictionary<Action<Tower>, Sprite> levelUpCallbackNames;
    public static List<Sprite> LevelUpSprites = new List<Sprite>();
    public static Tower twr;
    private Entity _enemy;
    public Entity Enemy { get => _enemy;
        set
        {
            if (value == null)
            {
                _enemy = FindEnemy(this, agroRadius, enemiesCanShooted);
                return;
            }
            _enemy = value;
        }
    }
    private float time;
    private Dictionary<float, Entity> enemiesCanShooted = new Dictionary<float, Entity>();
    private Player _player = Player.instance;
    private float rotationAngle = 0f;
    private float ResultRotationAngle
    {
        get => rotationAngle;
        set
        {
            if (value > 360) value -= 360;
            if (value < 0) value += 360;
            rotationAngle = value;
        }
    }
    public Tower() => twr = this;
    new protected void Awake()
    {
        base.Awake();
        Mesh pMesh = gameObject.GetComponentInChildren<MeshFilter>().mesh; //надо поместить меш в объект и ссылатьс€ на этот объект с мешем, а не на сам меш.
        missle.GetComponent<Projectile>().MeshHolder = new MeshHolder(pMesh);
        Producer = gameObject;
        Source = this.FindSource();
        onEntityDeath += OnEntityDeath;
        foreach (var levelUp in levelUps)
            levelUpCallbacks.Add(lUCLinks[levelUp]);
        foreach (var eff in starterEffects)
        {
            onStart += effectLinks[eff][0];
            travel += effectLinks[eff][1];
            onEnd += effectLinks[eff][2];
        }
        if (!tower)
            tower = gameObject;//это че ваще бл€ть?
        entities.Add(this);
        missle.GetComponent<Projectile>().damage = damage;
        cost = new Resources(costs[0], costs[1], costs[2], costs[3]);
        upgradeCost = new Resources(0, 0, 0, 0);
        chance = new Chances(chances[0], chances[1], chances[2], chances[3], chances[4], chances[5], chances[6], chances[7]);
        GenerateUps(); //потом удалить                                                                                       зава х
    }
    public static void LoadSprite()
    {
        SpriteLoad spriteLoad = UnityEngine.Resources.Load<SpriteLoad>("SpriteLevelUp/LevelUpSprites");
        LevelUpSprites.AddRange(spriteLoad.sprites);
        levelUpCallbackNames = new()
        {
              { FireUp , Player.instance.levelUpSprites[0] },
            { ColdUp , Player.instance.levelUpSprites[1] },
            { LightningUp , Player.instance.levelUpSprites[2] },
            { VoidUp , Player.instance.levelUpSprites[3] },
            { PhysUp , Player.instance.levelUpSprites[4] },
            { RangeUp, Player.instance.levelUpSprites[5] },
            { AttackSpUp, Player.instance.levelUpSprites[6] },
            { DoubleAttackUp, Player.instance.levelUpSprites[7] },
            { FractionUp , Player.instance.levelUpSprites[8] },
            { SplashUp, Player.instance.levelUpSprites[9] },
            { BounceUp, Player.instance.levelUpSprites[10] },
            { PuddleUp, Player.instance.levelUpSprites[11] },
            { FireConvert, Player.instance.levelUpSprites[12] },
            { ColdConvert, Player.instance.levelUpSprites[13] },
            { LightningConvert, Player.instance.levelUpSprites[14] },
            { PhysicalConvert, Player.instance.levelUpSprites[15] },
            { ProjectileSpeedDown, Player.instance.levelUpSprites[16] },
            { ProjectileSpeedUp, Player.instance.levelUpSprites[17] },
        };
        Debug.Log(LevelUpSprites.Count);
    }

    private void OnDestroy()
    {
        entities.Remove(this);
    }
    private void FixedUpdate()
    {
        //enemy = FindEnemy(this, agroRadius, enemiesCanShooted);
    }
    new protected void Update()
    {
        base.Update();
        Enemy = FindEnemy(this, agroRadius, enemiesCanShooted);
        ResultRotationAngle += attackSpeed * Time.deltaTime * 30;
        Quaternion newDir;
        if (Enemy)
        {
            time += Time.deltaTime;
            newDir = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (Enemy.gameObject.transform.position - tower.transform.position), 3.14f, 0)), attackSpeed);

            if (time > 1 / attackSpeed)
            {
                Vector3 fromWhere = Source.Transform.position;
                Projectile pMissle = missle.GetComponent<Projectile>();//ниху€себе
                Shoot(this, fromWhere, Enemy.transform.position + Enemy.Direction * Enemy.speed / (projSpeed / 10), projSpeed, pMissle,
                    chance, onStart, travel, onEnd, new List<Entity>(), missle.transform.localScale, pMissle.damage); 
                if (UnityEngine.Random.Range(1, 99) > chance.doubleAttack)
                    time = 0f;
            }
        }
        else
            newDir = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, ResultRotationAngle, 0), 0.05f);
        transform.rotation = newDir;
    }
    private void OnEntityDeath(Entity sender) =>
        enemiesCanShooted.Remove(enemiesCanShooted.FirstOrDefault(s => s.Value == sender).Key);
    private void GenerateUps()
    {
        firstUp = UnityEngine.Random.Range(0, levelUpCallbacks.Count);
        secondUp = 0;
        while (true)
        {
            secondUp = UnityEngine.Random.Range(0, levelUpCallbacks.Count);
            if (secondUp == firstUp) continue;
            else break;
        }
    }

    private void OnDrawGizmos()
    {
        if (Enemy)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Enemy.transform.position, new Vector3(Enemy.transform.position.x, Enemy.transform.position.y + 10f, Enemy.transform.position.z));
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Enemy.transform.position + Enemy.Direction * Enemy.speed / (projSpeed / 10),
                new Vector3(Enemy.transform.position.x, Enemy.transform.position.y + 10f, Enemy.transform.position.z) + Enemy.Direction * Enemy.speed / (projSpeed / 10));
        }
    }
    //перекинуть эти методы в Entity
   
}
