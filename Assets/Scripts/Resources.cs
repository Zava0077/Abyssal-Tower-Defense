using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Resources
{
    public Resources(int gold,int wood,int stone,int voidEsences) 
    { 
        this.gold = gold;
        this.wood = wood;
        this.stone = stone;
        this.voidEsences = voidEsences;
    }
    public int gold;
    public int wood;
    public int stone;
    public int voidEsences;

    public bool Subtract(Resources cost)
    {
        Resources resources = Camera.main.GetComponent<Player>().resources;
        for(int i = 0; i < cost.GetType().GetFields().Length; i++)
        {
            if((int)resources.GetType().GetFields()[i].GetValue(resources) < (int)cost.GetType().GetFields()[i].GetValue(cost))
            {
                Debug.Log("что-то не хватает");
                return false;
            }
        }
        resources.gold -= cost.gold;
        resources.wood -= cost.wood;
        resources.stone -= cost.stone;
        resources.voidEsences -= cost.voidEsences;
        return true;
    }
}
