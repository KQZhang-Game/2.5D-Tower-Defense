using UnityEngine;

public static class DamageCalculator
{
    /// <summary>
    /// 基础伤害计算
    /// </summary>
    /// <param name="atk">攻击者攻击力</param>
    /// <param name="def">受击者防御力</param>
    /// <param name="damageRatio">伤害倍率(默认1，可做暴击/减速增伤/Buff)</param>
    /// <returns>最终扣血量</returns>
    public static int CalcDamage(int atk, int def, float damageRatio = 1f)
    {
        float baseDmg = atk * 100f / (100f + def);
        float finalDmg = baseDmg * damageRatio;
        return Mathf.Max(1, Mathf.CeilToInt(finalDmg));
    }

    // 重载：直接传两边配置类
    /// <summary>
    /// 用配置表直接计算伤害
    /// </summary>
    /// <param name="attacker">攻击者配置</param>
    /// <param name="defender">受击者配置</param>
    /// <param name="ratio">伤害倍率</param>
    /// <returns>最终伤害</returns>
    public static int CalcDamage(UnitBase attacker, UnitBase defender, float ratio = 1f)
    {
        if (attacker is TowerBase)
        {
            return CalcDamage((attacker as TowerBase).towerConfig.Attack,
                (defender as EnemyBase).enemyConfig.Defense, ratio);
        }
        else
        {
            return CalcDamage((attacker as EnemyBase).enemyConfig.Attack,
                (defender as TowerBase).towerConfig.Defense, ratio);
        }

    }
}