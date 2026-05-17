using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBlockTower : TowerBase
{
    private Move moveComp;
    private List<Move> moves = new List<Move>();
    private int blockCount;
    private EnemyBase currentBlockEnemy;

    // 检测重叠敌人的半径（和你的碰撞体大小一致即可）
    private float checkBlockRadius = 0.8f;

    protected override void Init()
    {
        base.Init();
        OnDead += OnDie;
        blockCount = towerConfig.BlockCount;
    }

    protected override void Attack()
    {
        if (!isAttack)
        {
            isAttack = true;
            attackCoroutine = StartCoroutine(Shoot());
        }
    }

    protected override void StopAttack()
    {
        if (isAttack)
        {
            isAttack = false;
            towerHead.rotation = originRotation;
            StopCoroutine(attackCoroutine);
        }
    }

    IEnumerator Shoot()
    {
        while (isDeploy && isAttack && currentTarget != null)
        {
            Bullet bullet = BulletPool.Instance.GetBullet(this);
            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = towerHead.transform.rotation;
            yield return new WaitForSeconds(towerConfig.AttackInterval);
        }
        isAttack = false;
    }
    public override void Deploy()
    {
        base.Deploy();
        // 部署完成后，主动检查并阻挡重叠的敌人
        CheckOverlapEnemyOnDeploy();
    }

    // 主动检测部署时重叠的敌人（解决初始重叠不触发Trigger的问题）
    private void CheckOverlapEnemyOnDeploy()
    {
        if (blockCount <= 0) return;

        // 检测当前位置重叠的所有敌人
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, checkBlockRadius, enemyLayerMask);

        foreach (var col in hitEnemies)
        {
            if (blockCount <= 0) break;
            HandleBlockEnemy(col);
        }
    }

    private void HandleBlockEnemy(Collider other)
    {
        if (moves.Contains(other.GetComponent<Move>())) return; // 避免重复阻挡

        moveComp = other.GetComponent<Move>();
        currentBlockEnemy = other.GetComponent<EnemyBase>();

        if (currentBlockEnemy != null)
            currentBlockEnemy.OnDead += () => { blockCount++; };

        if (moveComp != null)
        {
            moveComp.PauseMove();
            moves.Add(moveComp);
            blockCount--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (blockCount > 0 && isDeploy && other.CompareTag("Enemy"))
        {
            HandleBlockEnemy(other);
        }
    }

    private void OnDie()
    {
        foreach (var move in moves)
        {
            move.ContinueMove();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnDead -= OnDie;
    }
}