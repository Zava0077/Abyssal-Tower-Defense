using System.Collections;
using System.Collections.Generic;
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
        Debug.Log(moveSpeed);
        float moveDirection = Input.GetAxis("Horizontal");
        
        if(camera.transform.position.x <= 99 && moveDirection == 1)
        {
            camera.transform.position = camera.transform.position + new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);
        }
        else if(camera.transform.position.x >= -99 && moveDirection == -1)
        {
            camera.transform.position = camera.transform.position + new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);
        }
    }
}
