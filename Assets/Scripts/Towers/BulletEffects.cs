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
    public virtual IEnumerator OnStart(GameObject proj)
    {
        yield return null;

    }
    public virtual IEnumerator Travel(GameObject proj)
    {
        yield return null;
    }
    public virtual IEnumerator End(GameObject proj)
    {
        yield return null;

    }
}
