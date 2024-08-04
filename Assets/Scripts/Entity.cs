using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Entity : MonoBehaviour
{

    public float maxHealth;
    public float health;
    public Resistances resistances;
    public Transform transform;
    public List<Status> statuses = new List<Status>();
    public List<float> _damage = new List<float>();
    public List<float> _resist = new List<float>();
    public static List<GameObject> shadows = new List<GameObject>();
    public Damage damage;
    public float attackSpeed;
    public float projSpeed;
    public float multiplierTakeDamage;
    public float agroRadius;
    public static Entity _entity;
    [SerializeField] Material damageMat;
    Color defaultColor;
    public Entity()
    {
        _entity = this;
    }

    public void Awake()
    {
        damage = new Damage(_damage[0], _damage[1], _damage[2], _damage[3], _damage[4]);
        resistances = new Resistances(_resist[0], _resist[1], _resist[2], _resist[3], _resist[4]);
        
        if(gameObject.GetComponent<Renderer>())
        {
            //gameObject.GetComponent<Renderer>().materials[1] = damageMat;
            defaultColor = gameObject.GetComponent<Renderer>().materials[0].color;
        }
    }
    public void Death()
    {
        Destroy(gameObject);
    }
    public void Update()
    {
        foreach(Status status in statuses)
            status.DoStatus();
     
        if (health <= 0)
            Death();
    }
    public IEnumerator ColorChanger()
    {
        if (gameObject.GetComponent<Renderer>())
        {
            gameObject.GetComponent<Renderer>().materials[0].color = new Color(255, 0, 0, 75);
            yield return new WaitForSeconds(0.1f);
            gameObject.GetComponent<Renderer>().materials[0].color = defaultColor;
        }
    }
   
}
