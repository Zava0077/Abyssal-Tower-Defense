using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Fading : MonoBehaviour
{
    public Projectile producer { get; set; }
    public float liveTime;
    public float timer = 0f;
    public Color color;
    Renderer renderer;
    private void Awake()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }
    private void OnEnable()
    {
        renderer.material.color = color;
        StartCoroutine(DeathSentence(liveTime));
    }
    private void OnDisable()
    {
        StopCoroutine(DeathSentence(liveTime));
    }
    IEnumerator DeathSentence(float sec)
    {
        yield return new WaitForSeconds(sec);
        gameObject.SetActive(false);
    }
    void Update()
    {
        //timer += Time.deltaTime;
        //if (timer > liveTime)
        //    transform.parent.gameObject.SetActive(false);
        //gameObject.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, ((liveTime - timer) / liveTime) * 255);
    }
}
