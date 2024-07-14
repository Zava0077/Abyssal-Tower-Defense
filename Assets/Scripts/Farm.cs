using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Entity
{
    public Resources resCost;
    [SerializeField] List<int> costs = new List<int>();
    public Sprite spriteButton;
    [SerializeField] private float timeToGainRes;
    private Resources resGain;
    [SerializeField] List<int> gains = new List<int>();
    float timer = 0f;

    private void Awake()
    {
        _entity.Awake();
        resCost = new Resources(costs[0], costs[1], costs[2], costs[3]);
        resGain = new Resources(gains[0], gains[1], gains[2], 0);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _entity.Update();
        timer += Time.deltaTime;
        if (timer >= timeToGainRes)
        {
            Camera.main.GetComponent<Player>().resources.Gain(resGain);
            timer = 0f;
        }
    }
}
