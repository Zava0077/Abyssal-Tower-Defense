using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera camera;
    public float gold;
    public float wood;
    public float stone;
    public float voidEssence;
    private float moveSpeed;
    private float speed = 40f;
    public GameObject tower;
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
                    PlaceTower(tower, hit.transform.gameObject);
            }
        }
    }

    void PlaceTower(GameObject tower, GameObject place)
    {
        GameObject _tower = Instantiate(tower);
        _tower.transform.position = place.transform.position;
        Debug.Log(place.name);
    }
}
