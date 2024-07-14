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

    public int[] GetMassive()
    {
        int[] massive = new int[4];
        massive[0] = gold;
        massive[1] = wood;
        massive[2] = stone;
        massive[3] = voidEsences;
        return massive;
    }

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

    public void Gain(Resources gain)
    {
        Resources resources = Camera.main.GetComponent<Player>().resources;
        resources.gold += gain.gold;
        resources.wood += gain.wood;
        resources.stone += gain.stone;
        resources.voidEsences += gain.voidEsences;
    }
}
