using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BulletEffects : MonoBehaviour
{
    public object Clone()
    {
        return MemberwiseClone();
    }
    public abstract void OnStart(GameObject proj);
    public abstract void Travel(GameObject proj);
    public abstract void End(GameObject proj);
}
