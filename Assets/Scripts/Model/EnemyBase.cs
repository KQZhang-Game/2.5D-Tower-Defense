using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyBase : UnitBase
{
    [HideInInspector]public EnemyConfig enemyConfig;
    [HideInInspector]public List<Transform> movePath = new List<Transform>();
    [HideInInspector]public UnityEvent OnDestroyAction;
    protected override void Init()
    {
        GameDataManager.Instance.SetEnemyData(this);
        currentHp = enemyConfig.MaxHp;
        OnTakeDamage += SubHp;
    }
    public void SetMovePath(List<Transform> path)
    {
        movePath.Clear();
        movePath = path;    
    }
    public List<Transform> GetMovePath()
    {
        return movePath;
    }
    protected virtual void Update()
    {
        switch (enemyConfig.AttackRangeShape)
        {
            case AttackRangeShape.Rectangle:
                hitCount = Physics.OverlapBoxNonAlloc(transform.position + rangeCenterPointOffset,
                                        new Vector3(enemyConfig.AttackRangeLength, enemyConfig.AttackRangeHeight, enemyConfig.AttackRangeWidth) / 2,
                                        inRangeEnemies, transform.rotation, playerLayerMask, QueryTriggerInteraction.Collide);
                break;
            case AttackRangeShape.Sphere:
                hitCount = Physics.OverlapSphereNonAlloc(transform.position + rangeCenterPointOffset,
                                        enemyConfig.AttackRangeRadius, inRangeEnemies, playerLayerMask, QueryTriggerInteraction.Collide);
                break;
            default:
                break;
        }
        if (hitCount > 0)
        {
            if (towerHead != null)
            {
                if (GetTargetIndex() != -1)
                {
                    currentTarget = GameDataManager.Instance.colliderMap[inRangeEnemies[GetTargetIndex()]] as TowerBase;

                }
                if ((currentTarget as TowerBase).GetIsDeploy)
                {
                    towerHead.rotation = Quaternion.Slerp(towerHead.rotation,
                                                Quaternion.LookRotation(currentTarget.transform.position - towerHead.position),
                                                Time.deltaTime * enemyConfig.RotateSpeed);
                    Attack();
                }

            }
        }
        else
        {
            currentTarget = null;
            StopAttack();
        }
    }
    protected virtual int GetTargetIndex()
    {
        if (hitCount == 0) return -1;
        if (hitCount == 1)
        {
            //判断敌方有没有部署
            if (!(GameDataManager.Instance.colliderMap[inRangeEnemies[0]] as TowerBase)) return -1;
            return 0;
        }
        if (hitCount >= 2)
        {
            TowerBase minHPEnemy = GameDataManager.Instance.colliderMap[inRangeEnemies[0]] as TowerBase;
            //排除还抓在手上没有部署的炮塔
            int minIndex = 0;
            if (!minHPEnemy.GetIsDeploy)
            {
                for (int i = 0; i < hitCount; i++)
                {
                    if (inRangeEnemies[i] != null)
                    {
                        minHPEnemy = GameDataManager.Instance.colliderMap[inRangeEnemies[i]] as TowerBase;
                        minIndex = i;
                    }
                }
            }
            for (int i = minIndex; i < hitCount; i++)
            {
                if (inRangeEnemies[i] == null ||
                    !(GameDataManager.Instance.colliderMap[inRangeEnemies[i]] as TowerBase).GetIsDeploy)
                    continue;
                if ((GameDataManager.Instance.colliderMap[inRangeEnemies[i]] as TowerBase).GetCurrentHP <
                    minHPEnemy.GetCurrentHP)
                {
                    minHPEnemy = GameDataManager.Instance.colliderMap[inRangeEnemies[i]] as TowerBase;
                    minIndex = i;
                }
            }
            return minIndex;
        }
        return -1;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnDestroyAction?.Invoke();
        OnTakeDamage -= SubHp;
    }
    private void SubHp(int damage)
    {
        UIHpBar.UpdateUIHpBar(enemyConfig.MaxHp, currentHp, damage);
    }
}
