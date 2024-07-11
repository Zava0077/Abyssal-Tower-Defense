using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    public static DoDamage dodamge;
    public DoDamage()
    {
        dodamge = this;
    }

    public static void DealDamage(Entity TakeDamage, Entity _DoDamage, Damage damage)
    {
        TakeDamage.health -= damage._fire * (1 - TakeDamage.resistances._fire);
        TakeDamage.health -= damage._lightning * (1 - TakeDamage.resistances._lightning);
        TakeDamage.health -= damage._cold * (1 - TakeDamage.resistances._cold);
        TakeDamage.health -= damage._void * (1 - TakeDamage.resistances._void);
        TakeDamage.health -= damage._physical * (1 - TakeDamage.resistances._physical);
        TakeDamage.ColorChanger();
    }
   
}
