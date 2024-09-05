using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Camera camera;
    public static Player instance;
    Player()
    {
        instance = this;
    }
    #region Pools
    public static ObjectPool<Puddle> nPuddles = new ObjectPool<Puddle>(128);
    public static ObjectPool<Explotion> nExplosions = new ObjectPool<Explotion>(128);
    public static ObjectPool<Projectile> nProjectile = new ObjectPool<Projectile>(1024);
    public static ObjectPool<Fading> nShadows = new ObjectPool<Fading>(64);
    #endregion
    private float moveSpeed;
    private float speed = 40f;
    [SerializeField] private List<GameObject> _res;
    public Resources resources = new Resources(50,50,50,0);
    [SerializeField] public List<GameObject> Towers;
    public Sprite[] levelUpSprites;
    [SerializeField] public GameObject explotion;
    [SerializeField] public GameObject puddle;
    [SerializeField] public GameObject particleShadow;
    public float levelUpBonus = 2f;
    public List<GameObject> Ferms;
    RaycastHit[] hits;
    public AudioSource shoot, laser, expl, expl2, bounce, fraction, pudd, hit, pierce, create, hot, snow, homing, electric;


    private void Start()
    {
        Camera.main.GetComponentInChildren<Image>().gameObject.SetActive(false);
    }
    private void Update()
    {
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));//дорого
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = speed * 2;
        }
        else
        {
            moveSpeed = speed;
        }
        float moveDirection = Input.GetAxisRaw("Vertical") * -1;
        if (camera.transform.position.x <= 230 && moveDirection == 1)
        {
            camera.transform.position = camera.transform.position + new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);
        }
        else if (camera.transform.position.x >= -99 && moveDirection == -1)
        {
            camera.transform.position = camera.transform.position + new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);
        }
        if (hits.Length > 0 && !CanvasController.cnv.menu.IsActive())
        {
            foreach(RaycastHit hit in hits)
                if (hit.transform.gameObject.tag == "Tower\'sPlace")
                {
                    if (Input.GetMouseButtonDown(0))
                        hit.transform.gameObject.GetComponentInChildren<CanvasController>().Show();
                    //hit.transform.gameObject.GetComponentInChildren<CanvasController>().showAgro = true;
                }
        }
        for(int i = 0; i < _res.Count; i++)
        {
            _res[i].GetComponentInChildren<Text>().text = resources.GetMassive()[i].ToString();
        }
    }
}
