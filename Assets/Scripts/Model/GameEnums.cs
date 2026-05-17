using System.Xml.Serialization;

public enum E_Deployable_Type
{
    [XmlEnum("0")] PLATFORM,   // 高台
    [XmlEnum("1")] FLOOR       // 地面
}

public enum TowerType
{
    [XmlEnum("0")] SlowTower,      // 减速塔
    [XmlEnum("1")] AreaAttackTower,// 范围攻击塔
    [XmlEnum("2")] GroundBlockTower,// 地面阻挡塔
    [XmlEnum("3")] NormalEnemy,    // 普通敌人
    [XmlEnum("4")] HeavyEnemy,     // 重甲敌人
    [XmlEnum("5")]FastEnemy       // 快速敌人
}

public enum AttackRangeShape
{
    [XmlEnum("0")] Rectangle,  // 矩形
    [XmlEnum("1")] Sphere,     // 球形
}