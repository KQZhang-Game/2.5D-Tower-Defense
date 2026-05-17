using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : EnemyBase
{
    protected override void Attack()
    {
        if (!isAttack)
        {
            isAttack = true;
            attackCoroutine = StartCoroutine(Shoot());
        }
    }

    protected override void Init()
    {
        base.Init();
    }

    protected override void StopAttack()
    {
        if (isAttack)
        {
            isAttack = false;
            towerHead.rotation = transform.rotation;
            StopCoroutine(attackCoroutine);
        }
    }
    IEnumerator Shoot()
    {
        while ( isAttack && currentTarget != null)
        {
            yield return new WaitForSeconds(enemyConfig.AttackInterval);
            Bullet bullet = BulletPool.Instance.GetBullet(this);
            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = towerHead.transform.rotation;
        }
        isAttack = false;
    }
}
