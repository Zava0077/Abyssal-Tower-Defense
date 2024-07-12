using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera camera;
    public float voidEssence;
    private float moveSpeed;
    private float speed = 40f;
    public Resources resources = new Resources(50,50,50,0);
    [SerializeField] public List<GameObject> Towers;
    public Sprite[] levelUpSprites;
    [SerializeField] public GameObject explotion;
    [SerializeField] public GameObject puddle;
    public float levelUpBonus = 2f;
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = speed * 2;
        }
        else
        {
            moveSpeed = speed;
        }
        float moveDirection = Input.GetAxis("Horizontal");
        if (camera.transform.position.x <= 99 && moveDirection == 1)
        {
            camera.transform.position = camera.transform.position + new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);
        }
        else if (camera.transform.position.x >= -99 && moveDirection == -1)
        {
            camera.transform.position = camera.transform.position + new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                if (hit.transform.gameObject.tag == "Tower\'sPlace")
                    hit.transform.gameObject.GetComponentInChildren<CanvasController>().canvas.gameObject.SetActive(true);
            }
        }
    }

    //void PlaceTower(GameObject tower, GameObject place)
    //{
    //    GameObject _tower = Instantiate(tower);
    //    _tower.transform.position = place.transform.position;
    //    Debug.Log(place.name);
    //}
}
