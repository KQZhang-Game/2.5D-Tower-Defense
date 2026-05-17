using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class EnemyConfig
{
    [XmlElement("TowerType")]
    public TowerType TowerType;
    [XmlElement("DeployableType")]
    public E_Deployable_Type DeployableType;
    [XmlElement("AttackRangeShape")]
    public AttackRangeShape AttackRangeShape;

    [XmlElement("MaxHp")]
    public int MaxHp;
    [XmlElement("Defense")]
    public int Defense;
    [XmlElement("Attack")]
    public int Attack;
    [XmlElement("AttackInterval")]
    public float AttackInterval;
    [XmlElement("MoveSpeed")]
    public float MoveSpeed;
    [XmlElement("RotateSpeed")]
    public float RotateSpeed;

    [XmlElement("AttackRangeLength")]
    public int AttackRangeLength;
    [XmlElement("AttackRangeWidth")]
    public int AttackRangeWidth;
    [XmlElement("AttackRangeHeight")]
    public int AttackRangeHeight;
    [XmlElement("AttackRangeRadius")]
    public int AttackRangeRadius;
}

[XmlRoot("EnemyConfigTable")]
public class EnemyConfigTable
{
    [XmlElement("EnemyConfig")]
    public List<EnemyConfig> EnemyList = new List<EnemyConfig>();
}