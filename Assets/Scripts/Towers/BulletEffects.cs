using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletEffects : MonoBehaviour
{
    public object Clone()
    {
        return MemberwiseClone();
    }
    public virtual void OnStart(GameObject proj)
    {

    }
    public virtual void Travel(GameObject proj)
    {

    }
    public virtual void End(GameObject proj)
    {

    }
}
