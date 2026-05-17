using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnitBase : MonoBehaviour
{
    public TowerType towerType;
    public Vector3 rangeCenterPointOffset = Vector3.zero;
    public Transform towerHead;
    public Transform shootPoint;

    protected bool isAttack = false;
    protected UnitBase currentTarget;
    protected Collider[] inRangeEnemies = new Collider[50];
    protected int hitCount = 0;
    protected Coroutine attackCoroutine;

    protected Collider _collider;
    public GameObject deadEff;
    public float deadEffKeepTime;
    public int currentHp;
    public int GetCurrentHP => currentHp;
    protected int playerLayerMask;
    protected int enemyLayerMask;
    public UnityAction<int> OnTakeDamage;
    public UnityAction OnDead;
    protected abstract void Attack();
    protected abstract void StopAttack();

    protected UnitHpBar UIHpBar;
    protected Vector3 UIHpBarPos;
    protected virtual void Dead()
    {
        if (deadEff != null)
        {
            GameObject effObj = Instantiate(deadEff, transform.position, transform.rotation);
            AudioSource sound = effObj.GetComponent<AudioSource>();
            if (sound != null)
            {
                AudioManager.Instance.SetSoundState(sound);
            }
        }
        Destroy(this.gameObject);
    }
    private void Awake()
    {
        Init();
        //此处只做通用初始化
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        _collider = this.GetComponent<Collider>();
        if (_collider != null && !GameDataManager.Instance.colliderMap.ContainsKey(_collider))
        {
            GameDataManager.Instance.colliderMap[_collider] = this;
        }
        OnDead += Dead;
        OnTakeDamage += TakeDamage;
        UIHpBar = UIHpBarPool.Instance.GetHpBar();
        UIHpBar.transform.position = transform.position;
    }
    /// <summary>
    /// 子类初始化方法
    /// </summary>
    protected abstract void Init();
    protected virtual void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0)
        {
            currentHp = 0;
            OnDead?.Invoke();
        }
    }
    protected virtual void OnDestroy()
    {
        StopAttack();
        UIHpBarPool.Instance.Release(UIHpBar);
        GameDataManager.Instance.colliderMap.Remove(_collider);
    }
    protected void LateUpdate()
    {
        if (UIHpBar == null) return;
        if (UIHpBarPos == Vector3.zero)
        {
            UIHpBar.transform.position = transform.position + Vector3.up * 2;
        }
        else
        {
            UIHpBar.transform.position = UIHpBarPos;
        }
    }
}
