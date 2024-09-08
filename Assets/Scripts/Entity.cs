using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public delegate void MobDelete(object sender);
interface IDamagable
{
    void GetDamage(Damage damage);
}
public class Entity : MonoBehaviour, IDamagable
{
    public static Entity entity;
    public static List<Entity> entities = new List<Entity>();
    public static event MobDelete onEntityDeath;
    [Header("Stats")]
    public float maxHealth;
    public float health; 
    public float attackSpeed;
    public float projSpeed;
    public float multiplierTakeDamage;
    public float agroRadius;
    public Damage damage;
    public Resistances resistances;
    public Transform transform;
    public List<Status> statuses = new List<Status>();
    public List<float> _damage = new List<float>();
    public List<float> _resist = new List<float>();
    public static List<GameObject> shadows = new List<GameObject>();
    Renderer renderer;
    [SerializeField] private Material damageMat;
    private Color defaultColor;

    public Entity()
    {
        entity = this;
    }

    public void Awake()
    {
        damage = new Damage(_damage[0], _damage[1], _damage[2], _damage[3], _damage[4]);
        resistances = new Resistances(_resist[0], _resist[1], _resist[2], _resist[3], _resist[4]);
        renderer = GetComponent<Renderer>();

        if (gameObject.GetComponent<Renderer>())
            defaultColor = gameObject.GetComponent<Renderer>().materials[0].color;
    }
    private void Death()
    {
        onEntityDeath?.Invoke(this);
        Destroy(gameObject);
    }
    public void Update()
    {
        foreach (Status status in statuses)
            status.DoStatus();

        if (health <= 0)
            Death();
    }
    public IEnumerator ColorChanger()
    {
        if (renderer)
        {
            renderer.materials[0].color = new Color(255, 0, 0, 75);
            yield return new WaitForSeconds(0.1f);
            renderer.materials[0].color = defaultColor;
        }
    }
    public void GetDamage(Damage damage)
    {
        health -= damage._fire * (1 - resistances._fire);
        health -= damage._lightning * (1 - resistances._lightning);
        health -= damage._cold * (1 - resistances._cold);
        health -= damage._void * (1 - resistances._void);
        health -= damage._physical * (1 - resistances._physical);
        StartCoroutine(ColorChanger());

    }
}
