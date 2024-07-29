using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static LevelUp;

public class Projectile : MonoBehaviour
{
    public Projectile(Damage damage, Vector3 target, GameObject owner, float agroRadius, Chances chance)
    {
        this.damage = damage;
        this.target = target;
        this.owner = owner;
        this.agroRadius = agroRadius;
        this.chance = chance;
    }
    public Chances chance;
    public float agroRadius;
    public GameObject owner;
    public Vector3 target;
    public Vector3 targetMemory;
    public Vector3 projection;
    public Vector3 position;
    public List<Mob> prevEnemy = new List<Mob>();
    public Damage damage;
    public float projSpeed;
    public float archMultiplier;
    public float distance;
    public float projHeight = 0f;
    public float liveTime = 0f;
    public float timeNeed;
    public float testTimer;
    public bool collidable;
    public List<BulletEffects> effects; //��, ��� ���������� �� ����� �����, � ������ � � �����
    private void Awake()
    {
        projHeight = archMultiplier;
    }
    private void Start()
    {
        if (target != null)
        {
            position = transform.position;
            distance = Vector3.Distance(position, target);
            targetMemory = target;
            timeNeed = distance / (Vector3.forward * projSpeed).magnitude * 1.25f;
            testTimer = 0f;
            collidable = true;
        }
        foreach (BulletEffects effect in effects)
            effect.OnStart(gameObject);  //�������������� ������� ������� � ������ ������,��������, ��������� ���������� �����������, ��������� ��������.
    }
    private void Update()
    {
        liveTime += Time.deltaTime;
        foreach (BulletEffects effect in effects)
            effect.Travel(gameObject);//�������������� ������� ������� �� ����� �����,��������, �� ��� ������� �������� ������
    }
    private void OnCollisionStay(Collision collision)
    {
        prevEnemy ??= new List<Mob>();
        if (collidable)
        {
            if (prevEnemy.Contains(collision.gameObject.GetComponent<Mob>()))
                return;
            if (collision.gameObject.tag == "Enemy" && damage != null)
            {
                DoDamage.DealDamage(collision.gameObject.GetComponent<Entity>(), null, damage);
                prevEnemy.Add(collision.gameObject.GetComponent<Mob>());
            }
            if (collision.gameObject.tag != "Effect")
            {
                foreach (BulletEffects effect in effects)
                    effect.End(gameObject);
                if (chance.pierce < Random.Range(1, 100))
                {
                    Destroy(gameObject);
                    enabled = true;
                }
            }
            liveTime = 0f;
        }
    }
}
