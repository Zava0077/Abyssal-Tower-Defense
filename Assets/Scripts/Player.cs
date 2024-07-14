using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Camera camera;
    private float moveSpeed;
    private float speed = 40f;
    [SerializeField] private List<GameObject> _res;
    public Resources resources = new Resources(50,50,50,0);
    [SerializeField] public List<GameObject> Towers;
    public List<GameObject> Ferms;
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
        float moveDirection = Input.GetAxis("Vertical") * -1;
        if (camera.transform.position.x <= 230 && moveDirection == 1)
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
                {
                    hit.transform.gameObject.GetComponentInChildren<CanvasController>().canvas.gameObject.SetActive(true);
                }
                else if (hit.transform.gameObject.tag == "Farm\'sPlace")
                {
                    hit.transform.gameObject.GetComponentInChildren<CanvasControllerFerm>().canvas.gameObject.SetActive(true);
                }
                
            }
        }
        for(int i = 0; i < _res.Count; i++)
        {
            _res[i].GetComponentInChildren<Text>().text = resources.GetMassive()[i].ToString();
        }
    }

    //void PlaceTower(GameObject tower, GameObject place)
    //{
    //    GameObject _tower = Instantiate(tower);
    //    _tower.transform.position = place.transform.position;
    //    Debug.Log(place.name);
    //}
}
