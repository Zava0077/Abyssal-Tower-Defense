using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Entity
{
    public float speed;
    private void Awake()
    {
        _entity.Awake();
    }
    private void Update()
    {
        _entity.Update();
    }
}
