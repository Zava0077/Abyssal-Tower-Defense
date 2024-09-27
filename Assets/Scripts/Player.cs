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


    private void Start()
    {
        Tower.LoadSprite();
        Camera.main.GetComponentInChildren<Image>().gameObject.SetActive(false);
        resources.Start();
    }
    private void Update()
    {
        //hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));//дорого
        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    moveSpeed = speed * 2;
        //}
        //else
        //{
        //    moveSpeed = speed;
        //}
        //float moveDirection = Input.GetAxisRaw("Vertical") * -1;
        //if (camera.transform.position.x <= 230 && moveDirection == 1)
        //{
        //    camera.transform.position = camera.transform.position + new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);
        //}
        //else if (camera.transform.position.x >= -99 && moveDirection == -1)
        //{
        //    camera.transform.position = camera.transform.position + new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);
        //}
    }
}
