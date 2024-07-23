using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

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
    public Mob prevEnemy;
    public Damage damage;
    public float projSpeed;
    public float archMultiplier;
    public float distance;
    public float projHeight = 0f;
    public float liveTime = 0f;
    public float timeNeed;
    public float testTimer;
    public int bounces = 0;
    public List<BulletEffects> effects; //��, ��� ���������� �� ����� ����� � � �����
    private void Awake()
    {
        projHeight = archMultiplier;
    }
    public void Start()
    {
        if(target != null)
        {
            distance = Vector3.Distance(gameObject.transform.position, target);
            targetMemory = target;
            timeNeed = distance / (Vector3.right * projSpeed).magnitude;
            position = transform.position;
            testTimer = 0f;
        }
        foreach (BulletEffects effect in effects)
        {
            if (effect)
                effect.OnStart(gameObject);//�������������� ������� ������� �� ����� �����,��������, �� ��� ������� �������� ������
        }
    }
    private void Update()
    {
        liveTime += Time.deltaTime;
        foreach (BulletEffects effect in effects)
        {
            if(effect)
                effect.Travel(gameObject);//�������������� ������� ������� �� ����� �����,��������, �� ��� ������� �������� ������
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.GetComponent<Projectile>())
            gameObject.GetComponent<Projectile>().enabled = true;
            if (prevEnemy == collision.gameObject.GetComponent<Mob>() && prevEnemy != null)
                return;
            if (collision.gameObject.tag == "Enemy" && damage != null)
            {
                DoDamage.DealDamage(collision.gameObject.GetComponent<Entity>(), null, damage);
                prevEnemy = collision.gameObject.gameObject.GetComponent<Mob>();
            }
        if (collision.gameObject.tag != "Effect")
        {
            foreach (BulletEffects effect in effects)
                    effect.End(gameObject); //�������������� ������� ������� � ����� �����, ��������, �����.
            if (chance.pierce < Random.Range(1, 100) || collision.gameObject.tag == "Ground")
                Destroy(gameObject);
        }
        liveTime = 0f;
    }
}
