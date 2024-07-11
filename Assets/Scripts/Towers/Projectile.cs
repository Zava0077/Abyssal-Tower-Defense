using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Projectile : MonoBehaviour
{
    public Projectile(Damage damage, Vector3 target, GameObject owner, float agroRadius)
    {
        this.damage = damage;
        this.target = target;
        this.owner = owner;
        this.agroRadius = agroRadius;
    }
    public float agroRadius;
    public GameObject owner;
    public Vector3 target;
    public Vector3 targetMemory;
    public Vector3 projection;
    public Mob prevEnemy;
    public Damage damage;
    public float projSpeed;
    public float archMultiplier;
    float distance;
    float deltaDistance = 0f;
    float projHeight = 0f;
    float timeNeed;
    public float testTimer;
    public List<BulletEffects> effects; //то, что происходит во время полёта и в конце
    private void Awake()
    {
        projHeight = archMultiplier;
    }
    private void Start()
    {
        if(target != null)
        {
            distance = Vector3.Distance(this.gameObject.transform.position, target);
            targetMemory = target;
            timeNeed = distance / (Vector3.right * projSpeed).magnitude;
            testTimer = 0f;
        }
    }
    private void Update()
    {
        testTimer += Time.deltaTime;
        if (testTimer > timeNeed)
            projHeight = -100;
        else
        if (testTimer > timeNeed / 2 && projHeight > 0)
            projHeight *= -1;
        transform.position += transform.right * projSpeed * Time.deltaTime;
        transform.position += new Vector3(0, projHeight * Time.deltaTime, 0);
        
        foreach (BulletEffects effect in effects)
        {
            effect.Travel(gameObject);//дополнительные эффекты снаряда во время полёта,например, за ним остаётся ядовитое облако
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (prevEnemy == other.GetComponent<Mob>() && prevEnemy != null)
            return;
        if (other.gameObject.tag == "Enemy" && damage != null)
        {
            DoDamage.DealDamage(other.GetComponent<Entity>(), null, damage);
            prevEnemy = other.gameObject.GetComponent<Mob>();
        }
        if (other.tag != "Effect")
        {
            foreach (BulletEffects effect in effects)
                effect.End(gameObject); //дополнительные эффекты снаряда в конце полёта, например, взрыв.
            Debug.Log(timeNeed + " " + testTimer);
            Destroy(gameObject);
        }
    }
}
