using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class GameDataManager
{
    public static readonly GameDataManager Instance = new GameDataManager();
    public Dictionary<Collider, UnitBase> colliderMap = new Dictionary<Collider, UnitBase>();
    public MusicData MusicData { private set; get; }
    private string fileName = "MusicData";
    private TowerConfigTable towerConfigTable = new TowerConfigTable();
    private List<TowerConfig> towerConfigList = new List<TowerConfig>();
    private EnemyConfigTable enemyConfigTable = new EnemyConfigTable();
    private List<EnemyConfig> enemyConfigList = new List<EnemyConfig>();
    public List<TowerConfig> TowerConfigList => towerConfigList;
    private GameDataManager()
    {
        towerConfigTable = XmlDataMgr.Instance.LoadData(typeof(TowerConfigTable), "TowerConfig") as TowerConfigTable;
        enemyConfigTable = XmlDataMgr.Instance.LoadData(typeof(EnemyConfigTable), "EnemyConfig") as EnemyConfigTable;
        if (enemyConfigTable != null)
        {
            enemyConfigList = enemyConfigTable.EnemyList;
        }
        if (towerConfigTable != null)
        {
            towerConfigList = towerConfigTable.TowerList;
        }
        MusicData = XmlDataMgr.Instance.LoadData(typeof(MusicData), fileName) as MusicData;
        if (MusicData == null)
        {
            MusicData = new MusicData();
            MusicData.isMusicOpen = true;
            MusicData.isSoundOpen = true;
            MusicData.musicVolume = 1;
            MusicData.soundVolume = 1;
            XmlDataMgr.Instance.SaveData(MusicData, fileName);
        }
    }
    public void ChangMusicState(bool value)
    {
        MusicData.isMusicOpen = value;
        XmlDataMgr.Instance.SaveData(MusicData, fileName);
    }
    public void ChangSoundState(bool value)
    {
        MusicData.isSoundOpen = value;
        XmlDataMgr.Instance.SaveData(MusicData, fileName);
    }
    public void ChangMusicVolume(float value)
    {
        MusicData.musicVolume = Mathf.Clamp01(value);
        XmlDataMgr.Instance.SaveData(MusicData, fileName);
    }
    public void ChangSoundVolume(float value)
    {
        MusicData.soundVolume = Mathf.Clamp01(value);
        XmlDataMgr.Instance.SaveData(MusicData, fileName);
    }
    public void SetTowerData(TowerBase tower)
    {
        foreach (var i in towerConfigList)
        {
            if (tower.towerType == i.TowerType)
            {
                tower.towerConfig = i;
                return;
            }
        }
    }
    public void SetEnemyData(EnemyBase enemy)
    {
        foreach (var i in enemyConfigList)
        {
            if (enemy.towerType == i.TowerType)
            {
                enemy.enemyConfig = i;
                return;
            }
        }
    }
}
