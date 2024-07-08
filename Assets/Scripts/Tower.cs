using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Tower : Entity
{
    public GameObject missle;
    GameObject[] enemies;
    Entity enemy;
    [SerializeField] GameObject tower;
    float time;
    private void Awake()
    {
        _entity.Awake();
    }
    private void Update()
    {
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
            newDir = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (tower.transform.position - transform.forward), 3.14f, 0)), Time.deltaTime * attackSpeed);//redo
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
    void Shoot(Entity target)
    {
        Quaternion rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        rotation.SetEulerAngles((3.14f / 180) * rotation.eulerAngles.x, (3.14f / 180) * rotation.eulerAngles.y - (3.14f/180) * 90, (3.14f / 180) * rotation.eulerAngles.z);
        GameObject _missle = Instantiate(missle, transform.position, rotation , transform.parent);
        _missle.GetComponent<Projectile>().target = target;
        _missle.GetComponent<Projectile>().damage = damage;
        _missle.GetComponent<Projectile>().projSpeed = projSpeed;
    }
}
