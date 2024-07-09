using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Dictionary<GameObject, bool> keyValuePairs = new Dictionary<GameObject, bool>();
    protected void Awake()
    {
        _entity.Awake();
        if(!tower)
            tower = GetComponent<GameObject>();
        cost = new Resources(costs[0], costs[1], costs[2], costs[3]);
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
        if (FindEnemy())
        {
            time += Time.deltaTime;
            newDir = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (enemy.gameObject.transform.position - tower.transform.position), 3.14f, 0)), attackSpeed);

            if (time > 1 / attackSpeed)
            {
                Shoot(enemy);
                time = 0f;
            }
        }
        else
            newDir = Quaternion.Lerp(transform.rotation,  Quaternion.Euler(0, currentRotationAngle, 0), 0.05f);
        transform.rotation = newDir;
    }
    bool FindEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); //мб переделать, а то слишком доху€ чекать
        foreach (var enemy in enemies)
            if (Vector3.Distance(enemy.transform.position, tower.transform.position) < agroRadius) 
            {
                this.enemy = enemy.GetComponent<Mob>();
                return true; 
            }
            else continue;
        time = 0f;
        return false;
    }

    public virtual void Shoot(Entity target)
    {
        Quaternion rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        rotation.SetEulerAngles((3.14f / 180) * rotation.eulerAngles.x, (3.14f / 180) * rotation.eulerAngles.y - (3.14f/180) * 90, (3.14f / 180) * rotation.eulerAngles.z);
        GameObject _missle = Instantiate(missle, transform.position, rotation , transform.parent);
        _missle.GetComponent<Projectile>().target = target;
        _missle.GetComponent<Projectile>().damage = damage;//new Damage(damage._fire * incDamage, damage._cold * incDamage, damage._lightning * incDamage, damage._void * incDamage,damage._physical * incDamage);
        _missle.GetComponent<Projectile>().projSpeed = projSpeed;
    }
}
