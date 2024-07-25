using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class LevelUp
{
    
    public delegate void LevelUpCallback(Tower tower);
    public LevelUpCallback StatUp; //Заменить конверт уронов на пробивку этим типом урона
    public static LevelUpCallback FireUp = (Tower tower) =>
    {
        tower.GetComponent<Tower>().damage._fire += Camera.main.GetComponent<Player>().levelUpBonus;
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback ColdUp = (Tower tower) =>
    {
        tower.GetComponent<Tower>().damage._cold += Camera.main.GetComponent<Player>().levelUpBonus;
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback LightningUp = (Tower tower) =>
    {
        tower.GetComponent<Tower>().damage._lightning += Camera.main.GetComponent<Player>().levelUpBonus;
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback VoidUp = (Tower tower) =>
    {
        tower.GetComponent<Tower>().damage._void += Camera.main.GetComponent<Player>().levelUpBonus / 50f;
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback PhysUp = (Tower tower) =>
    {

        tower.GetComponent<Tower>().damage._physical += Camera.main.GetComponent<Player>().levelUpBonus;
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback RangeUp = (Tower tower) =>
    {
        tower.agroRadius += Camera.main.GetComponent<Player>().levelUpBonus;
        if (tower.agroRadius > 80)
            tower.levelUpCallbacks.Remove(RangeUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback AttackSpUp = (Tower tower) =>
    {
        tower.attackSpeed += Camera.main.GetComponent<Player>().levelUpBonus/10;//~
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback DoubleAttackUp = (Tower tower) =>
    {
        tower.chance.doubleAttack += Camera.main.GetComponent<Player>().levelUpBonus;//50
        if (tower.chance.doubleAttack > 50)
            tower.levelUpCallbacks.Remove(DoubleAttackUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback FractionUp = (Tower tower) =>
    {
        if (!tower.GetComponent<Fraction>())
            tower.effects.Add(tower.AddComponent<Fraction>());
        else
            tower.chance.shatter += Camera.main.GetComponent<Player>().levelUpBonus;//25
        if (tower.chance.shatter > 25)
            tower.levelUpCallbacks.Remove(FractionUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback SplashUp = (Tower tower) =>
    {
        if (!tower.GetComponent<Bomb>())
            tower.effects.Add(tower.AddComponent<Bomb>());
        else
        tower.chance.splash += Camera.main.GetComponent<Player>().levelUpBonus;//75
        if (tower.chance.splash > 75)
            tower.levelUpCallbacks.Remove(SplashUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback ProjectileSpeedDown = (Tower tower) =>
    {
        tower.projSpeed -= Camera.main.GetComponent<Player>().levelUpBonus;
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback BounceUp = (Tower tower) =>
    {
        if (!tower.GetComponent<Bounce>())
            tower.effects.Add(tower.AddComponent<Bounce>());
        else
            tower.chance.bounce += Camera.main.GetComponent<Player>().levelUpBonus;//25
        if (tower.chance.bounce > 25)
            tower.levelUpCallbacks.Remove(BounceUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback PuddleUp = (Tower tower) =>
    {
        if (!tower.GetComponent<WaterPocket>())
            tower.effects.Add(tower.AddComponent<WaterPocket>());
        else
            tower.chance.puddle += Camera.main.GetComponent<Player>().levelUpBonus;//75
        if (tower.chance.puddle > 75)
            tower.levelUpCallbacks.Remove(PuddleUp);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback FireConvert = (Tower tower) =>
    {
        tower.GetComponent<Tower>().damage._fire += tower.GetComponent<Tower>().damage._cold + tower.GetComponent<Tower>().damage._lightning + tower.GetComponent<Tower>().damage._void + tower.GetComponent<Tower>().damage._physical;
        tower.GetComponent<Tower>().damage._cold = 0;
        tower.GetComponent<Tower>().damage._lightning = 0;
        tower.GetComponent<Tower>().damage._void = 0;
        tower.GetComponent<Tower>().damage._physical = 0;
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(FireConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(LightningConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(ColdConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(PhysicalConvert);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback ColdConvert = (Tower tower) =>
    {

        tower.GetComponent<Tower>().damage._cold += tower.GetComponent<Tower>().damage._fire + tower.GetComponent<Tower>().damage._lightning + tower.GetComponent<Tower>().damage._void + tower.GetComponent<Tower>().damage._physical;
        tower.GetComponent<Tower>().damage._fire = 0;
        tower.GetComponent<Tower>().damage._lightning = 0;
        tower.GetComponent<Tower>().damage._void = 0;
        tower.GetComponent<Tower>().damage._physical = 0;
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(FireConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(LightningConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(ColdConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(PhysicalConvert);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback LightningConvert = (Tower tower) =>
    {
        tower.GetComponent<Tower>().damage._lightning += tower.GetComponent<Tower>().damage._fire + tower.GetComponent<Tower>().damage._cold + tower.GetComponent<Tower>().damage._void + tower.GetComponent<Tower>().damage._physical;
        tower.GetComponent<Tower>().damage._fire = 0;
        tower.GetComponent<Tower>().damage._cold = 0;
        tower.GetComponent<Tower>().damage._void = 0;
        tower.GetComponent<Tower>().damage._physical = 0;
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(FireConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(LightningConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(ColdConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(PhysicalConvert);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
    public static LevelUpCallback PhysicalConvert = (Tower tower) =>
    {
        tower.GetComponent<Tower>().damage._physical += tower.GetComponent<Tower>().damage._fire + tower.GetComponent<Tower>().damage._cold + tower.GetComponent<Tower>().damage._lightning + tower.GetComponent<Tower>().damage._void;
        tower.GetComponent<Tower>().damage._fire = 0;
        tower.GetComponent<Tower>().damage._cold = 0;
        tower.GetComponent<Tower>().damage._lightning = 0;
        tower.GetComponent<Tower>().damage._void = 0;
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(FireConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(LightningConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(ColdConvert);
        tower.GetComponent<Tower>().levelUpCallbacks.Remove(PhysicalConvert);
        tower.levelUpsRemain--;
        tower.updateLvlUp = true;
    };
}
