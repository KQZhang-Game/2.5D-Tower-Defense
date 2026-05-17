using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackTower : TowerBase
{
    public GameObject attackEff;
    protected override void Init()
    {
        base.Init();
        attackEff.SetActive(false);
    }
    protected override void Attack()
    {
        if (!isAttack)
        {
            attackEff.SetActive(true);
            isAttack = true;
            attackCoroutine = StartCoroutine(Shoot());
        }
    }
    protected override void StopAttack()
    {
        if (isAttack)
        {
            attackEff.SetActive(false);
            isAttack = false;
            StopCoroutine(attackCoroutine);
        }
    }

    protected override void Update()
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
            if (isDeploy)
            {
                Attack();
            }
        }
        else
        {
            inRangeEnemies = new Collider[50];
            StopAttack();
        }
    }
    IEnumerator Shoot()
    {
        while (isDeploy && isAttack)
        {
            foreach (var enemy in inRangeEnemies)
            {
                if (enemy == null) continue;
                EnemyBase enemyBase = GameDataManager.Instance.colliderMap[enemy] as EnemyBase;
                enemyBase.OnTakeDamage?.Invoke(DamageCalculator.CalcDamage(this,enemyBase));
            }
            yield return new WaitForSeconds(towerConfig.AttackInterval);
        }
        isAttack = false;
    }
}
