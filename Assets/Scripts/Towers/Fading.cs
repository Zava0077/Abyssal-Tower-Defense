using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Fading : MonoBehaviour, IProducable
{
    public Projectile producer { get; set; }
    public float liveTime;
    public float timer = 0f;
    public Color color;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > liveTime)
            transform.parent.gameObject.SetActive(false);
        gameObject.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, ((liveTime - timer) / liveTime) * 255);
    }
}
