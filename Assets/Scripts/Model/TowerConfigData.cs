using System.Collections.Generic;
using System.Xml.Serialization;
public class TowerConfig
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
    [XmlElement("BlockCount")]
    public int BlockCount;

    [XmlElement("AttackRangeLength")]
    public int AttackRangeLength;
    [XmlElement("AttackRangeWidth")]
    public int AttackRangeWidth ;
    [XmlElement("AttackRangeHeight")]
    public int AttackRangeHeight;
    [XmlElement("AttackRangeRadius")]
    public int AttackRangeRadius;
    [XmlElement("RotateSpeed")]
    public float RotateSpeed;
    [XmlElement("DeployCost")]
    public int DeployCost;
}

[XmlRoot("TowerConfigTable")]
public class TowerConfigTable
{
    [XmlElement("TowerConfig")]
    public List<TowerConfig> TowerList = new List<TowerConfig>();
}