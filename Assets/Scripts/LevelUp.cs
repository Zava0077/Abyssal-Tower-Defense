using System;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public static class LevelUp
{
    public static readonly Action<Tower,Action<Tower>> Controller = (Tower tower, Action<Tower> deleg) =>
    {
        if (!Player.instance.resources.Subtract(tower.upgradeCost))
        {
            Debug.Log("Не хватает ресурсов для прокачки!");
            return;
        }
        deleg(tower);
        tower.upgradeCost.Gain(5 + (int)(tower.currentLevel * 0.25f),5 + (int)(tower.currentLevel * 0.25f), 5 + (int)(tower.currentLevel * 0.25f), 0);
    };
    public static Action<Tower> FireUp = (Tower tower) =>
    {
        tower.damage._fire += Player.instance.levelUpBonus;
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> ColdUp = (Tower tower) =>
    {
        tower.damage._cold += Player.instance.levelUpBonus;
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> LightningUp = (Tower tower) =>
    {
        tower.damage._lightning += Player.instance.levelUpBonus;
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> VoidUp = (Tower tower) =>
    {
        tower.damage._void += Player.instance.levelUpBonus / 50f;
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> PhysUp = (Tower tower) =>
    {

        tower.damage._physical += Player.instance.levelUpBonus;
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> RangeUp = (Tower tower) =>
    {
        tower.agroRadius += Player.instance.levelUpBonus;
        if (tower.agroRadius > 40)
            tower.levelUpCallbacks.Remove(RangeUp);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> AttackSpUp = (Tower tower) =>
    {
        tower.attackSpeed += Player.instance.levelUpBonus / 10;//~
        if (tower.attackSpeed >= 10)
            tower.levelUpCallbacks.Remove(AttackSpUp);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> DoubleAttackUp = (Tower tower) =>
    {
        tower.chance.doubleAttack += Player.instance.levelUpBonus;//50
        if (tower.chance.doubleAttack > 50)
            tower.levelUpCallbacks.Remove(DoubleAttackUp);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> FractionUp = (Tower tower) =>
    {
        if (!BulletEffects.Has(BulletEffects.fractionEnd, tower))
            tower.onEnd += BulletEffects.fractionEnd;
        else
            tower.chance.shatter += Player.instance.levelUpBonus;//25
        if (tower.chance.shatter > 25)
            tower.levelUpCallbacks.Remove(FractionUp);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> SplashUp = (Tower tower) =>
    {
        if (!BulletEffects.Has(BulletEffects.explotionEnd, tower))
            tower.onEnd += BulletEffects.explotionEnd;
        else
            tower.chance.splash += Player.instance.levelUpBonus;//75
        if (tower.chance.splash > 75)
            tower.levelUpCallbacks.Remove(SplashUp);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> ProjectileSpeedDown = (Tower tower) =>
    {
        tower.projSpeed -= Player.instance.levelUpBonus;
        if (tower.projSpeed <= 10)
            tower.levelUpCallbacks.Remove(ProjectileSpeedDown);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> ProjectileSpeedUp = (Tower tower) =>
    {
        tower.projSpeed += Player.instance.levelUpBonus;
        if (tower.projSpeed >= 35)
            tower.levelUpCallbacks.Remove(ProjectileSpeedUp);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> BounceUp = (Tower tower) =>
    {
        if (!BulletEffects.Has(BulletEffects.bounceEnd, tower))
            tower.onEnd += BulletEffects.bounceEnd;
        else
            tower.chance.bounce += Player.instance.levelUpBonus;//25
        if (tower.chance.bounce > 25)
            tower.levelUpCallbacks.Remove(BounceUp);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> PuddleUp = (Tower tower) =>
    {
        if (!BulletEffects.Has(BulletEffects.puddleEnd, tower))
            tower.onEnd += BulletEffects.puddleEnd;
        else
            tower.chance.puddle += Player.instance.levelUpBonus;//75
        if (tower.chance.puddle > 75)
            tower.levelUpCallbacks.Remove(PuddleUp);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> FireConvert = (Tower tower) =>
    {
        tower.damage._fire += tower.damage._cold + tower.damage._lightning + tower.damage._void + tower.damage._physical;
        tower.damage._cold = 0;
        tower.damage._lightning = 0;
        tower.damage._void = 0;
        tower.damage._physical = 0;
        tower.levelUpCallbacks.Remove(FireConvert);
        tower.levelUpCallbacks.Remove(LightningConvert);
        tower.levelUpCallbacks.Remove(ColdConvert);
        tower.levelUpCallbacks.Remove(PhysicalConvert);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> ColdConvert = (Tower tower) =>
    {

        tower.damage._cold += tower.damage._fire + tower.damage._lightning + tower.damage._void + tower.damage._physical;
        tower.damage._fire = 0;
        tower.damage._lightning = 0;
        tower.damage._void = 0;
        tower.damage._physical = 0;
        tower.levelUpCallbacks.Remove(FireConvert);
        tower.levelUpCallbacks.Remove(LightningConvert);
        tower.levelUpCallbacks.Remove(ColdConvert);
        tower.levelUpCallbacks.Remove(PhysicalConvert);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> LightningConvert = (Tower tower) =>
    {
        tower.damage._lightning += tower.damage._fire + tower.damage._cold + tower.damage._void + tower.damage._physical;
        tower.damage._fire = 0;
        tower.damage._cold = 0;
        tower.damage._void = 0;
        tower.damage._physical = 0;
        tower.levelUpCallbacks.Remove(FireConvert);
        tower.levelUpCallbacks.Remove(LightningConvert);
        tower.levelUpCallbacks.Remove(ColdConvert);
        tower.levelUpCallbacks.Remove(PhysicalConvert);
        tower.LevelUpsRemain--;

    };
    public static Action<Tower> PhysicalConvert = (Tower tower) =>
    {
        tower.damage._physical += tower.damage._fire + tower.damage._cold + tower.damage._lightning + tower.damage._void;
        tower.damage._fire = 0;
        tower.damage._cold = 0;
        tower.damage._lightning = 0;
        tower.damage._void = 0;
        tower.levelUpCallbacks.Remove(FireConvert);
        tower.levelUpCallbacks.Remove(LightningConvert);
        tower.levelUpCallbacks.Remove(ColdConvert);
        tower.levelUpCallbacks.Remove(PhysicalConvert);
        tower.LevelUpsRemain--;

    };
}
