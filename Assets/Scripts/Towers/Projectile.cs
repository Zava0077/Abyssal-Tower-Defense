using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Projectile(Damage damage, Entity target)
    {
        this.damage = damage;
        this.target = target;
    }
    public Entity target;
    public Damage damage;
    public float projSpeed;
    public List<BulletEffects> statuses = new List<BulletEffects>();//��, ��� ���������� �� ����� ����� � � �����
    private void Update()
    {
        /*
        foreach (BulletEffects effect in statuses)
        {
            effect.Travel();//�������������� ������� ������� �� ����� �����,��������, �� ��� ������� �������� ������
        }
        */
        transform.position += transform.right * projSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() == target)
            DoDamage.DealDamage(target, null, damage);
        /*
        foreach (BulletEffects effect in statuses)
        {
            effect.End(); //�������������� ������� ������� � ����� �����, ��������, �����.
        }
        */
        Destroy(this.gameObject);
    }
}
