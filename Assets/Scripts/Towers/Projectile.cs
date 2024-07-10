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
        }
    }
    private void Update()
    {
        projection = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        deltaDistance = Vector3.Distance(projection, targetMemory);
        if (deltaDistance < distance / 2 && projHeight > 0)
            projHeight *= -1;
        transform.position += transform.right * projSpeed * Time.deltaTime;
        transform.position += new Vector3(0, projHeight * Time.deltaTime, 0);
        
        foreach (BulletEffects effect in effects)
        {
            effect.Travel();//дополнительные эффекты снаряда во время полёта,например, за ним остаётся ядовитое облако
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
        if(other.tag != "Effect")
        {
            foreach (BulletEffects effect in effects)
                effect.End(); //дополнительные эффекты снаряда в конце полёта, например, взрыв.
            Destroy(gameObject);
        }
    }
}
