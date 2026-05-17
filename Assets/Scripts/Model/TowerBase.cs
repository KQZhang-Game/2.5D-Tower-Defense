using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class TowerBase : UnitBase
{
    public TowerConfig towerConfig;
    protected bool isDeploy;
    protected bool isBlocking;
    public UnityEvent OnDeploy;
    public bool GetIsDeploy => isDeploy;
    protected Quaternion originRotation;
    protected override void Init()
    {
        GameDataManager.Instance.SetTowerData(this);
        originRotation = transform.rotation;
        OnTakeDamage += SubHp;
        currentHp = towerConfig.MaxHp;
        OnDeploy.AddListener(Deploy);
    }
    public void DrawAttackRange()
    {
        LineWriter.Instance.DrawLineInXZ(towerConfig.AttackRangeShape, transform.position + rangeCenterPointOffset, towerConfig.AttackRangeRadius
            , towerConfig.AttackRangeLength, towerConfig.AttackRangeWidth);
    }
    protected virtual void Update()
    {
        if (!isDeploy) return;
        switch (towerConfig.AttackRangeShape)
        {
            case AttackRangeShape.Rectangle:
                hitCount = Physics.OverlapBoxNonAlloc(transform.position + rangeCenterPointOffset,
                                        new Vector3(towerConfig.AttackRangeLength, towerConfig.AttackRangeHeight, towerConfig.AttackRangeWidth) / 2,
                                        inRangeEnemies, transform.rotation, enemyLayerMask, QueryTriggerInteraction.Collide);
                break;
            case AttackRangeShape.Sphere:
                hitCount = Physics.OverlapSphereNonAlloc(transform.position + rangeCenterPointOffset,
                                        towerConfig.AttackRangeRadius, inRangeEnemies, enemyLayerMask, QueryTriggerInteraction.Collide);
                break;
            default:
                break;
        }

        if (hitCount > 0)
        {
            if (towerHead != null)
            {
                //towerHead.LookAt(GameDataManager.Instance.colliderMap[inRangeEnemies[GetTargetIndex()]].transform);
                if (GetTargetIndex() != -1)
                {
                    currentTarget = GameDataManager.Instance.colliderMap[inRangeEnemies[GetTargetIndex()]] as EnemyBase;
                }
                if (isDeploy)
                {
                    towerHead.rotation = Quaternion.Slerp(towerHead.rotation,
                                            Quaternion.LookRotation(currentTarget.transform.position - towerHead.position),
                                            Time.deltaTime * towerConfig.RotateSpeed);
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
            return 0;
        }
        if (hitCount >= 2)
        {
            EnemyBase minHPEnemy = GameDataManager.Instance.colliderMap[inRangeEnemies[0]] as EnemyBase;
            int minIndex = 0;
            for (int i = 0; i < hitCount; i++)
            {
                if (inRangeEnemies[i] == null) continue;
                if (GameDataManager.Instance.colliderMap[inRangeEnemies[i]].GetCurrentHP < minHPEnemy.GetCurrentHP)
                {
                    minHPEnemy = GameDataManager.Instance.colliderMap[inRangeEnemies[i]] as EnemyBase;
                    minIndex = i;
                }
            }
            return minIndex;
        }
        return -1;
    }
    public virtual void Deploy()
    {
        isDeploy = true;
    }
    private void SubHp(int damage)
    {
        UIHpBar.UpdateUIHpBar(towerConfig.MaxHp, currentHp, damage);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnTakeDamage -= SubHp;
        OnDeploy.RemoveListener(Deploy);
    }
}
