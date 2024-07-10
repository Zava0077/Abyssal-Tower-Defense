using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Tower : Entity
{
    public GameObject missle;
    static GameObject[] enemies;
    Entity enemy;
    [SerializeField] GameObject tower;
    float time;
    public float incDamage = 1;
    public float incAttackSpeed;
    public float incHealth;
    private float currentRotationAngle = 0f;
    public Sprite spriteButton;
    public Resources cost;
    [SerializeField] List<int> costs = new List<int>();
    [SerializeField] List<int> chances = new List<int>();
    [SerializeField] Dictionary<GameObject, bool> keyValuePairs = new Dictionary<GameObject, bool>();
    public List<BulletEffects> effects = new List<BulletEffects>();
    Chances chance;

    protected void Awake()
    {
        _entity.Awake();
        if(!tower)
            tower = GetComponent<GameObject>();
        cost = new Resources(costs[0], costs[1], costs[2], costs[3]);
        chance = new Chances(chances[0], chances[1], chances[2], chances[3], chances[4], chances[5], chances[6]);
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
        currentRotationAngle += attackSpeed * Time.deltaTime * 30;
        if (currentRotationAngle >= 360f)
        {
            currentRotationAngle -= 360f;
        }
        _entity.Update();
        Quaternion newDir;
        Entity enemy = FindEnemy(tower, tower.GetComponent<Tower>().agroRadius);
        if (enemy)
        {
            time += Time.deltaTime;
            newDir = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (enemy.gameObject.transform.position - tower.transform.position), 3.14f, 0)), attackSpeed);

            if (time > 1 / attackSpeed)
            {
                Shoot(this.gameObject, enemy.GetComponent<Transform>().position, damage, missle, agroRadius);
                time = 0f;
            }
        }
        else
            newDir = Quaternion.Lerp(transform.rotation,  Quaternion.Euler(0, currentRotationAngle, 0), 0.05f);
        transform.rotation = newDir;
    }
    public static Entity FindEnemy(GameObject tower, float agroRadius, Mob lastEnemy = null)
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); //мб переделать, а то слишком доху€ чекать
        foreach (var enemy in enemies)
            if (Vector3.Distance(enemy.transform.position, tower.transform.position) < agroRadius) 
            {
                if (lastEnemy && enemy == lastEnemy.GetComponent<GameObject>()) continue;
                return enemy.GetComponent<Mob>();
            }
            else continue;
        if(tower.GetComponent<Tower>())
            tower.GetComponent<Tower>().time = 0f;
        return null;
    }

    public static void Shoot(GameObject turret,Vector3 target, Damage damage, GameObject missle, float agroRadius)
    {
        Quaternion rotation = new Quaternion(turret.transform.rotation.x, turret.transform.rotation.y, turret.transform.rotation.z, turret.transform.rotation.w);
        rotation.SetEulerAngles((3.14f / 180) * rotation.eulerAngles.x, (3.14f / 180) * rotation.eulerAngles.y - (3.14f / 180) * 90, (3.14f / 180) * rotation.eulerAngles.z);
        GameObject _missle = Instantiate(missle, turret.transform.position, Quaternion.identity , turret.transform.parent);
        _missle.transform.rotation = rotation;
        _missle.GetComponent<Projectile>().target = target;
        _missle.GetComponent<Projectile>().damage = damage;//new Damage(damage._fire * incDamage, damage._cold * incDamage, damage._lightning * incDamage, damage._void * incDamage,damage._physical * incDamage);
        _missle.GetComponent<Projectile>().owner = turret; //у бомбы нет энтити
        _missle.GetComponent<Projectile>().agroRadius = agroRadius;
        if (turret.GetComponent<Tower>())
            _missle.GetComponent<Projectile>().effects = turret.GetComponent<Tower>().effects;
        //else if(turret.GetComponent<Projectile>())
        //    _missle.GetComponent<Projectile>().effects = turret.GetComponent<Projectile>().effects;
        foreach (var _effect in _missle.GetComponent<Projectile>().effects)
            _effect.projectile = _missle;
        if (turret.GetComponent<Tower>())
            _missle.GetComponent<Projectile>().projSpeed = turret.GetComponent<Tower>().projSpeed;
        //else if (turret.GetComponent<Projectile>())
        //    _missle.GetComponent<Projectile>().projSpeed = turret.GetComponent<Projectile>().projSpeed;
    }
}
