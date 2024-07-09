using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Projectile : MonoBehaviour
{
    public Projectile(Damage damage, Entity target, Entity owner)
    {
        this.damage = damage;
        this.target = target;
        this.owner = owner;
    }
    public Entity owner;
    public Entity target;
    public Vector3 targetMemory;
    public Vector3 projection;
    public Damage damage;
    public float projSpeed;
    public float archMultiplier;
    float distance;
    float deltaDistance = 0f;
    float projHeight = 0f;
    public List<BulletEffects> statuses = new List<BulletEffects>();//то, что происходит во время полёта и в конце
    private void Awake()
    {
        projHeight = archMultiplier;
    }
    private void Start()
    {
        if(target)
        {
            distance = Vector3.Distance(this.gameObject.transform.position, target.GetComponent<Transform>().position);
            targetMemory = target.GetComponent<Transform>().position;
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
        
        foreach (BulletEffects effect in statuses)
        {
            effect.Travel();//дополнительные эффекты снаряда во время полёта,например, за ним остаётся ядовитое облако
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        foreach (BulletEffects effect in statuses)
        {
            effect.End(); //дополнительные эффекты снаряда в конце полёта, например, взрыв.
        }
        if (other.gameObject.GetComponent<Entity>() == target && damage != null)
            DoDamage.DealDamage(target, null, damage);
        Destroy(gameObject);
    }
}
