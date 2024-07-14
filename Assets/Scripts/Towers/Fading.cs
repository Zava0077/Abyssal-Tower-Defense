using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public float liveTime;
    public float timer = 0f;
    public Color color;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > liveTime)
            Destroy(transform.parent.gameObject);
        gameObject.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, ((liveTime - timer) / liveTime) * 255);
    }
}
