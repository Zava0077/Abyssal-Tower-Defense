using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chances : MonoBehaviour
{
    public float bounce;
    public float splash;
    public float puddle;
    public float shatter;
    public float doubleAttack;

    public float crit;
    public float status; //hz chto eto

    public float pierce;

    public Chances(float bounce, float splash, float puddle, float shatter, float doubleAttack, float crit, float status, float pierce)
    {
        this.bounce = bounce;
        this.splash = splash;
        this.puddle = puddle;
        this.shatter = shatter;
        this.doubleAttack = doubleAttack;
        this.crit = crit;
        this.status = status;
        this.pierce = pierce;
    }
}
