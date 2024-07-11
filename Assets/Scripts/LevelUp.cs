using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class LevelUp
{
    
    public delegate void LevelUpCallback(Tower tower, int type);
    public LevelUpCallback StatUp;
    public static LevelUpCallback DamageUp = (Tower tower, int type) =>
    {
        //object _object1 = (object)((float)(tower.damage.GetType().GetFields()[type].GetValue(tower.damage)) + Camera.main.GetComponent<Player>().levelUpBonus);
        //object _object = tower.damage.GetType().GetFields()[type].GetValue(tower.damage);
        //Debug.Log(_object.GetType().ToString() + " " + _object1.GetType().ToString());
        //FieldInfo[] fields = tower.damage.GetType().GetFields();
        //fields[type].SetValue(_object, _object1);//~
        switch(type)
        {
            case 0:
                tower.GetComponent<Tower>().damage._fire += Camera.main.GetComponent<Player>().levelUpBonus;
                break;
            case 1:
                tower.GetComponent<Tower>().damage._cold += Camera.main.GetComponent<Player>().levelUpBonus;
                break;
            case 2:
                tower.GetComponent<Tower>().damage._lightning += Camera.main.GetComponent<Player>().levelUpBonus;
                break;
            case 3:
                tower.GetComponent<Tower>().damage._void += Camera.main.GetComponent<Player>().levelUpBonus;
                break;
            case 4:
                tower.GetComponent<Tower>().damage._physical += Camera.main.GetComponent<Player>().levelUpBonus;
                break;
        }
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback RangeUp = (Tower tower, int type) =>
    {
        tower.agroRadius += Camera.main.GetComponent<Player>().levelUpBonus;
        if (tower.agroRadius > 80)
            tower.levelUpCallbacks.Remove(RangeUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback AttackSpUp = (Tower tower, int type) =>
    {
        tower.attackSpeed += Camera.main.GetComponent<Player>().levelUpBonus/10;//~
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback DoubleAttackUp = (Tower tower, int type) =>
    {
        tower.chance.doubleAttack += Camera.main.GetComponent<Player>().levelUpBonus;//50
        if (tower.chance.doubleAttack > 50)
            tower.levelUpCallbacks.Remove(DoubleAttackUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback FractionUp = (Tower tower, int type) =>
    {
        if (tower.chance.shatter == 0)
            tower.effects.Add(tower.AddComponent<Fraction>());
        else
            tower.chance.shatter += Camera.main.GetComponent<Player>().levelUpBonus;//25
        if (tower.chance.shatter > 25)
            tower.levelUpCallbacks.Remove(FractionUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback SplashUp = (Tower tower, int type) =>
    {
        if (tower.chance.splash == 0)
            tower.effects.Add(tower.AddComponent<Bomb>());
        else
        tower.chance.splash += Camera.main.GetComponent<Player>().levelUpBonus;//75
        if (tower.chance.splash > 75)
            tower.levelUpCallbacks.Remove(SplashUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback BounceUp = (Tower tower, int type) =>
    {
        if (tower.chance.bounce == 0)
            tower.effects.Add(tower.AddComponent<Bounce>());
        else
            tower.chance.bounce += Camera.main.GetComponent<Player>().levelUpBonus;//25
        if (tower.chance.bounce > 25)
            tower.levelUpCallbacks.Remove(BounceUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback PuddleUp = (Tower tower, int type) =>
    {
        if (tower.chance.puddle == 0)
            tower.effects.Add(tower.AddComponent<WaterPocket>());
        else
            tower.chance.puddle += Camera.main.GetComponent<Player>().levelUpBonus;//75
        if (tower.chance.puddle > 75)
            tower.levelUpCallbacks.Remove(PuddleUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback DamageConvert = (Tower tower, int type) =>
    {
        //float sum = 0f;
        //foreach (var damage in tower.damage.GetType().GetFields())
        //    sum += (float)damage.GetValue(tower.damage);
        //foreach (var damage in tower.damage.GetType().GetFields())
        //    damage.SetValue(damage, (object)0f);
        //object @object = (object)((float)(tower.damage.GetType().GetFields()[type].GetValue(tower.damage)) + Camera.main.GetComponent<Player>().levelUpBonus);
        //tower.damage.GetType().GetFields()[type].SetValue(tower.damage.GetType().GetFields()[type], (object)sum);//~
        switch (type)
        {
            case 0:
                tower.GetComponent<Tower>().damage._fire += tower.GetComponent<Tower>().damage._cold + tower.GetComponent<Tower>().damage._lightning + tower.GetComponent<Tower>().damage._void + tower.GetComponent<Tower>().damage._physical;
                tower.GetComponent<Tower>().damage._cold += 0;
                tower.GetComponent<Tower>().damage._lightning += 0;
                tower.GetComponent<Tower>().damage._void += 0;
                tower.GetComponent<Tower>().damage._physical += 0;
                break;
            case 1:
                tower.GetComponent<Tower>().damage._cold += tower.GetComponent<Tower>().damage._fire + tower.GetComponent<Tower>().damage._lightning + tower.GetComponent<Tower>().damage._void + tower.GetComponent<Tower>().damage._physical;
                tower.GetComponent<Tower>().damage._fire += 0;
                tower.GetComponent<Tower>().damage._lightning += 0;
                tower.GetComponent<Tower>().damage._void += 0;
                tower.GetComponent<Tower>().damage._physical += 0;
                break;
            case 2:
                tower.GetComponent<Tower>().damage._lightning += tower.GetComponent<Tower>().damage._fire + tower.GetComponent<Tower>().damage._cold + tower.GetComponent<Tower>().damage._void + tower.GetComponent<Tower>().damage._physical;
                tower.GetComponent<Tower>().damage._fire += 0;
                tower.GetComponent<Tower>().damage._cold += 0;
                tower.GetComponent<Tower>().damage._void += 0;
                tower.GetComponent<Tower>().damage._physical += 0;
                break;
            case 3:
                tower.GetComponent<Tower>().damage._void += tower.GetComponent<Tower>().damage._fire + tower.GetComponent<Tower>().damage._cold + tower.GetComponent<Tower>().damage._lightning + tower.GetComponent<Tower>().damage._physical;
                tower.GetComponent<Tower>().damage._fire += 0;
                tower.GetComponent<Tower>().damage._cold += 0;
                tower.GetComponent<Tower>().damage._lightning += 0;
                tower.GetComponent<Tower>().damage._physical += 0;
                break;
            case 4:
                tower.GetComponent<Tower>().damage._physical += tower.GetComponent<Tower>().damage._fire + tower.GetComponent<Tower>().damage._cold + tower.GetComponent<Tower>().damage._lightning + tower.GetComponent<Tower>().damage._void;
                tower.GetComponent<Tower>().damage._fire += 0;
                tower.GetComponent<Tower>().damage._cold += 0;
                tower.GetComponent<Tower>().damage._lightning += 0;
                tower.GetComponent<Tower>().damage._void += 0;
                break;
        }
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
}
