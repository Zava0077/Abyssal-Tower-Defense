using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public AudioSource shoot, laser, expl, expl2, bounce, fraction, pudd, hit, pierce, create, hot, snow, homing, electric;

    [SerializeField] public List<Text> _res;
    public List<Farm> Farms;
    public List<Tower> Towers;

    public GameObject explotion;
    public GameObject puddle;
    public GameObject particleShadow;
    public CanvasController canvasController;

    public Camera camera;
    public static Player instance;
    Player()
    {
        instance = this;
    }
    #region Pools
    public static ObjectPool<Puddle> nPuddles = new ObjectPool<Puddle>(128);
    public static ObjectPool<Explotion> nExplosions = new ObjectPool<Explotion>(128);
    public static ObjectPool<Fading> nShadows = new ObjectPool<Fading>(64);
    #endregion
    private float moveSpeed;
    private float speed = 40f;
    public Resources resources = new Resources(50,50,50,0);
    public Sprite[] levelUpSprites;
    public float levelUpBonus = 2f;

    private void Awake()
    {
        Tower.LoadSprite();
    }

    private void Start()
    {
        Camera.main.GetComponentInChildren<Image>().gameObject.SetActive(false);
        resources.Start();
    }
}
