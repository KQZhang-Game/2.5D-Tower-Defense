using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override bool IsPersistent => false;
    private int cost;
    private int hp;
    [SerializeField] private int costPerSec = 1;
    public int Hp => hp;
    protected override void Awake()
    {
        base.Awake();
        Init();
        EnemyManager.Instance.OnEnterBlueDoor.AddListener(SubHp);
        StartCoroutine(ProduceCost());
    }
    public bool HasEnoughCost(int cost)
    {
        if (this.cost >= cost)
        {
            return true;
        }
        return false;
    }
    public void SubCost(int cost)
    {
        this.cost -= cost;
        GamePanel.Instance.UpdateCoinText(-cost);
    }
    private void Init()
    {
        hp = EnemyManager.Instance.CreateCount;
        GamePanel.Instance.UpdateHpText(hp);
        cost = 0;
    }
    private void SubHp()
    {
        hp -= 1;
        if (hp < 0) hp = 0;
        GamePanel.Instance.UpdateHpText(-1);
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        EnemyManager.Instance.OnEnterBlueDoor.RemoveListener(SubHp);
    }
    IEnumerator ProduceCost()
    {
        while (true)
        {
            cost += costPerSec;
            GamePanel.Instance.UpdateCoinText(costPerSec);
            yield return new WaitForSeconds(1);
        }
    }
}
